using System;
using Foundation;
using UIKit;

namespace Demo11
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public override UIWindow Window { get; set; }

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			UIViewController root;




			root = new UIViewController ();









			var window = new UIWindow (UIScreen.MainScreen.Bounds) {
				RootViewController = root,
			};
			window.MakeKeyAndVisible ();
			return true;
		}
	}
}

