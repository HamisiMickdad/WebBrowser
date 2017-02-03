using Android.App;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using System;
using Android.Runtime;
using Android.Views;
using Android.Graphics;

namespace WebBrowser
{
    [Activity(Label = "WebBrowser", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private WebView myWebView;
        private EditText myTxtURL;
        private WebClient myWebClient;
        private ProgressBar myProgressBar;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            myWebClient = new WebClient();

            myWebClient.myOnProgressChanged += (int state) =>
            {
                if (state == 0)
                {
                    //page loaded no progress bar visible
                    myProgressBar.Visibility = ViewStates.Invisible;
                }
                else
                {
                    myProgressBar.Visibility = ViewStates.Visible;
                }
            };

            myWebView = FindViewById<WebView>(Resource.Id.webView);
            myTxtURL = FindViewById<EditText>(Resource.Id.txtURL);
            myProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);

            myWebView.Settings.JavaScriptEnabled = true;
            myWebView.LoadUrl("https://www.google.com");
            myWebView.SetWebViewClient(myWebClient);

            myTxtURL.Click += MyTxtURL_Click;

        }
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (e.KeyCode == Keycode.Back)
            {
                myWebView.GoBack();
            }
            return true;
        }
        void MyTxtURL_Click(object sender, EventArgs e)
        {
            myWebClient.ShouldOverrideUrlLoading(myWebView, myTxtURL.Text);  
        }
    }

    public class WebClient : WebViewClient
    {
        public delegate void ToggleProgreeBar(int state);
            public ToggleProgreeBar myOnProgressChanged;

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            view.LoadUrl(url);
            return true;
        }
        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            if (myOnProgressChanged !=null)
            {
                myOnProgressChanged.Invoke(1);
            }
            base.OnPageStarted(view, url, favicon);
        }
        public override void OnPageFinished(WebView view, string url)
        {
            if(myOnProgressChanged != null)
            {
                myOnProgressChanged.Invoke(0);
            }
            base.OnPageFinished(view, url);
        }
    }

}

