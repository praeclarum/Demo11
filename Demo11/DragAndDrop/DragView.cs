using System;
using Foundation;
using UIKit;

namespace Demo11.DragAndDrop
{
	public class DragView : UIView
	{
		public string Name { get; }

		public DragView (string name)
		{
			Name = name;
		}

		public override void Draw (CoreGraphics.CGRect rect)
		{
			UIColor.FromHSB (80.0f / 360.0f, 0.1f, 0.9f).SetFill ();
			UIGraphics.RectFill (rect);

			UIColor.Black.SetColor ();
			var text = new NSString ($"Drag {Name}");
			var b = Bounds;
			b.Inflate (-22, -22);
			text.DrawString (b, UIFont.BoldSystemFontOfSize (24));
		}
	}
}
