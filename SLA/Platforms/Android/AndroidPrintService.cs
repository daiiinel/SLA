using Android.Content;
using Android.Print;
using SLA.Services;
using Microsoft.Maui.ApplicationModel;
using Android.Webkit;

namespace SLA.Platforms.Android;

public class AndroidPrintService : IPrintService
{
    public void PrintHtml(string html)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var context = Platform.CurrentActivity;
            if (context == null) 
                return;

            var webView = new global::Android.Webkit.WebView(context);
            webView.SetWebViewClient(new MyPrintWebViewClient(html));
            webView.LoadDataWithBaseURL(null, html, "text/html", "UTF-8", null);
        });
    }
}

// aux para manejar el evento de finalización de carga
internal class MyPrintWebViewClient : WebViewClient
{
    private readonly string _html;

    public MyPrintWebViewClient(string html)
    {
        _html = html;
    }

    // este es el método correcto q android llama cuando termina de cargar el html
    public override void OnPageFinished(global::Android.Webkit.WebView? view, string? url)
    {
        base.OnPageFinished(view, url);

        if (view == null) 
            return;

        var context = Platform.CurrentActivity;
        var printManager = context?.GetSystemService(Context.PrintService) as PrintManager;

        if (printManager != null)
        {
            // adaptador de impresión
            var printAdapter = view.CreatePrintDocumentAdapter("Formulario_2404");

            // se lanza la interfaz de impresión del sist
            printManager.Print("SLA_Registro", printAdapter, null);
        }
    }
}