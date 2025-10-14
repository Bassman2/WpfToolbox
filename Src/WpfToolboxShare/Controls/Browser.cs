//namespace WpfToolbox.Controls;

///// <summary>
///// Browser control
///// </summary>
//public class Browser : ContentControl
//{
//    // cannot derive directly from WebBrowser because it is sealed
//    private static ASCIIEncoding enc = new ASCIIEncoding();
//    private readonly WebBrowser browser;
//    private Uri currentUrl;
//    private string currentAuth; 

//    public Browser()
//    {
//        this.browser = new WebBrowser();
//        this.browser.Loaded += OnLoaded;
//        this.Content = this.browser;
//    }

//    private void OnLoaded(object sender, RoutedEventArgs e)
//    {
//        FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
//        object objComWebBrowser = fiComWebBrowser?.GetValue(this.browser);
//        objComWebBrowser?.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { true });
//    }

//    public static readonly DependencyProperty AuthenticationProperty =
//        DependencyProperty.Register("Authentication", typeof(string), typeof(Browser), new FrameworkPropertyMetadata(null));

//    public string Authentication
//    {
//        get { return (string)GetValue(AuthenticationProperty); }
//        set { SetValue(AuthenticationProperty, value); }
//    }


//    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(Browser), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));

//    public Uri Source
//    {
//        get { return (Uri)GetValue(SourceProperty); }
//        set { SetValue(SourceProperty, value); }
//    }

//    private static void OnSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
//    {
//        ((Browser)o).OnSourceChanged(e.NewValue as Uri);
//    }

//    private void OnSourceChanged(Uri newUrl)
//    {
//        if (this.currentUrl != newUrl || this.currentAuth != this.Authentication)
//        {
//            this.currentUrl = newUrl;
//            this.currentAuth = this.Authentication;

//            if (string.IsNullOrEmpty(this.Authentication))
//            {
//                browser.Navigate(newUrl);
//            }
//            else
//            {
//                browser.Navigate(newUrl, "", enc.GetBytes(this.Authentication), "Content-Type: application/x-www-form-urlencoded\r\n");
//            }
//        }
//    }
//}
