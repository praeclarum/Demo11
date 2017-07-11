using System;
using CoreGraphics;
using UIKit;
using Foundation;
using CoreML;
using CoreVideo;
using System.Threading;

namespace Demo11.MachineLearning
{
	public class MachineLearningViewController : UIViewController
	{
		MLModel model;

		UIImageView imageView = new UIImageView ();

		public MachineLearningViewController ()
		{
			Title = "Core ML";

			View = imageView;

			NavigationItem.RightBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Camera, PickImage);

			LoadModel ();
		}

		void PickImage (object sender, EventArgs _)
		{
			var keyWindow = UIApplication.SharedApplication.KeyWindow;

			var picker = new UIImagePickerController {
				AllowsEditing = false,
				SourceType = UIImagePickerControllerSourceType.Camera
			};

			picker.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;

			picker.FinishedPickingMedia += (s, e) => {
				DismissViewController (true, () => {
					keyWindow.MakeKeyWindow ();
					imageView.Image = e.OriginalImage;
					ThreadPool.QueueUserWorkItem (o => MakePrediction (e.OriginalImage));
				});
			};

			PresentViewController (picker, true, null);
		}

		void LoadModel ()
		{
			var modelUrl = NSBundle.MainBundle.GetUrlForResource ("SqueezeNet", "mlmodelc");

			model = MLModel.FromUrl (modelUrl, out var error);

			if (error != null) {
				Console.WriteLine ($"Error loading model: {error}");
			}
			else {
				Console.WriteLine ($"Loaded {model}");
			}
		}

		void MakePrediction (UIImage image)
		{
			//
			// Get an input from the image
			//
			IMLFeatureProvider input = CreateInput (image);

			//
			// Predict what's in the image
			//
			var output = model.GetPrediction (input, out var error);

			if (error != null) {
				Console.WriteLine ($"Error predicting: {error}");
				return;
			}

			var classLabel = output.GetFeatureValue ("classLabel").StringValue;

			//
			// Display everything
			//
			Console.WriteLine ($"Recognized:");
			foreach (NSString n in output.FeatureNames) {
				var value = output.GetFeatureValue (n);
				Console.WriteLine ($"  {n} == {value}");
			}

			var message = $"{classLabel.ToUpperInvariant ()} with probability {output.GetFeatureValue ("classLabelProbs").DictionaryValue[classLabel]}";

			Console.WriteLine (message);

			BeginInvokeOnMainThread (() => {
				var alert = UIAlertController.Create (message, "I hope it's right!", UIAlertControllerStyle.Alert);
				alert.AddAction (UIAlertAction.Create ("OK", UIAlertActionStyle.Default, (obj) => {
					DismissViewController (true, null);
				}));
				PresentViewController (alert, true, null);
			});
		}

		IMLFeatureProvider CreateInput (UIImage image)
		{
			var pixelBuffer = image.Resize (new CGSize (227, 227)).ToPixelBuffer ();

			var imageValue = MLFeatureValue.FromPixelBuffer (pixelBuffer);

			var inputs = new NSDictionary<NSString, NSObject> (new NSString ("image"), imageValue);

			return new MLDictionaryFeatureProvider (inputs, out var error);
		}
	}
}
