using Foundation;
using Lottie.Forms.iOS.Renderers;
using Plugin.CrossPlatformTintedImage.iOS;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace CheckCheque.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Xamarin.Forms.Forms.Init();

            AnimationViewRenderer.Init();
            TintedImageRenderer.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(application, launchOptions);
        }
    }
}

