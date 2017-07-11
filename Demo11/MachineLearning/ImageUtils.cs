using System;
using CoreGraphics;
using CoreVideo;
using UIKit;

namespace Demo11.MachineLearning
{
	public static class ImageUtils
	{
		public static UIImage Resize (this UIImage self, CGSize newSize)
		{
			UIGraphics.BeginImageContextWithOptions (new CGSize (width: newSize.Width, height: newSize.Height), true, 1.0f);
			self.Draw (new CGRect (x: 0, y: 0, width: newSize.Width, height: newSize.Height));
			var resizedImage = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return resizedImage;
		}

		public static CVPixelBuffer ToPixelBuffer (this UIImage self)
		{
			var width = self.Size.Width;

			var height = self.Size.Height;

			var attrs = new CVPixelBufferAttributes ();
			attrs.CGBitmapContextCompatibility = true;
			attrs.CGImageCompatibility = true;

			var resultPixelBuffer = new CVPixelBuffer ((int) (width),
					(int) (height),
					CVPixelFormatType.CV32ARGB,
											 attrs);

			resultPixelBuffer.Lock (CVPixelBufferLock.None);
			var pixelData = resultPixelBuffer.GetBaseAddress (0);

			var rgbColorSpace = CGColorSpace.CreateDeviceRGB ();

			var context = new CGBitmapContext (data: pixelData,
										  width: (int) (width),
										  height: (int) (height),
										  bitsPerComponent: 8,
									bytesPerRow: resultPixelBuffer.GetBytesPerRowOfPlane (0),
										  colorSpace: rgbColorSpace,
			                                   bitmapInfo: CGImageAlphaInfo.NoneSkipFirst);

			context.TranslateCTM (tx: 0, ty: height);
			context.ScaleCTM (sx: 1.0f, sy: -1.0f);

			UIGraphics.PushContext (context);

			self.Draw (new CGRect (x: 0, y: 0, width: width, height: height));

			UIGraphics.PopContext ();

			resultPixelBuffer.Unlock (CVPixelBufferLock.None);

			return resultPixelBuffer;
    	}
	}
}
