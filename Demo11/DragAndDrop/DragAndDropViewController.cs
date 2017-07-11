using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Demo11.DragAndDrop
{
	public class DragAndDropViewController : UIViewController
	{
		public DragAndDropViewController ()
		{
			Title = "Drag & Drop";
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.White;

			for (var i = 0; i < 6; i++) {
				DragView v = new DragView ($"{i}");
				AddView (v);
			}
		}

		void AddView (UIView view)
		{
			var b = View.Bounds;

			var nrows = 4;
			var ncols = 4;

			var topGap = (nfloat)64.0;
			var gap = (nfloat)11.0;

			var w = (b.Width - ((ncols + 1) * gap)) / ncols;
			var h = (b.Height - ((nrows + 1) * gap) - topGap) / nrows;

			var nexisting = View.Subviews.Length;

			var row = nexisting / ncols;
			var col = nexisting - (row * ncols);

			var x = (col + 1) * gap + col * w;
			var y = (row + 1) * gap + row * h + topGap;

			view.Frame = new CGRect (x, y, w, h);
			View.AddSubview (view);
		}
	}
}
