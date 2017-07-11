using System;
using UIKit;
using Foundation;
using CoreML;

namespace Demo11.MachineLearning
{
	public class MachineLearningViewController : UIViewController
	{
		MLModel model;

		public MachineLearningViewController ()
		{
			Title = "Core ML";

			LoadModel ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}

		void LoadModel ()
		{
			NSError error;
			var modelUrl = NSBundle.MainBundle.GetUrlForResource ("SqueezeNet", "mlmodelc");
			model = MLModel.FromUrl (modelUrl, out error);

			if (error != null) {
				Console.WriteLine ($"Error loading model: {error}");
			}
			else {
				Console.WriteLine ($"Loaded {model}");
			}
		}
	}
}
