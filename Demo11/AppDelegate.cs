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

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			//root = new UINavigationController (
				//new DragAndDrop.DragAndDropViewController ());

			root = new UINavigationController (
				new MachineLearning.MachineLearningViewController ());


			var window = new UIWindow (UIScreen.MainScreen.Bounds) {
				RootViewController = root,
			};
			window.MakeKeyAndVisible ();
			return true;
		}
	}
}

