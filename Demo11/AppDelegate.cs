using System;
using Foundation;
using UIKit;

namespace Demo11
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public override UIWindow Window { get; set; }

		UIViewController root;
		UINavigationController nav;

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			//root = new DragAndDrop.DragAndDropViewController ();

			//root = new MachineLearning.MachineLearningViewController ();

			root = new AwesomerReality.AwesomerRealityViewController ();

			nav = new UINavigationController (root);
			var window = new UIWindow (UIScreen.MainScreen.Bounds) {
				RootViewController = nav,
			};
			window.MakeKeyAndVisible ();
			return true;
		}
	}
}

