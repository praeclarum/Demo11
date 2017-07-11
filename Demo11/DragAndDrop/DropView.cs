using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Demo11.DragAndDrop
{
	public class DropView : UIView
	{
		public string Name { get; }

		UIColor defaultColor = UIColor.LightGray;

		public DropView (string name)
		{
			Name = name;

			BackgroundColor = defaultColor;

			EnableDropping ();
		}

		public override void Draw (CoreGraphics.CGRect rect)
		{
			UIColor.Black.SetColor ();
			var text = new NSString ($"Drop {Name}");
			var b = Bounds;
			b.Inflate (-22, -22);
			text.DrawString (b, UIFont.BoldSystemFontOfSize (24));
		}

		void EnableDropping ()
		{
			AddInteraction (new UIDropInteraction (new DropDelegate (this)));
		}

		class DropDelegate : UIDropInteractionDelegate
		{
			readonly DropView dropView;

			public DropDelegate (DropView dropView)
			{
				this.dropView = dropView;
			}

			public override bool CanHandleSession (UIDropInteraction interaction, IUIDropSession session)
			{
				Console.WriteLine ($"CanHandleSession ({interaction}, {session})");

				return session.CanLoadObjectsOfClass (new Class (typeof (NSString)));
			}

			public override void SessionDidEnter (UIDropInteraction interaction, IUIDropSession session)
			{
				Console.WriteLine ($"SessionDidEnter ({interaction}, {session})");

				BeginInvokeOnMainThread (() => {
					dropView.BackgroundColor = UIColor.Yellow;
				});
			}

			public override void SessionDidExit (UIDropInteraction interaction, IUIDropSession session)
			{
				Console.WriteLine ($"SessionDidExit ({interaction}, {session})");

				BeginInvokeOnMainThread (() => {
					dropView.BackgroundColor = dropView.defaultColor;
				});
			}

			public override UIDropProposal SessionDidUpdate (UIDropInteraction interaction, IUIDropSession session)
			{
				Console.WriteLine ($"SessionDidUpdate ({interaction}, {session})");

				return new UIDropProposal (UIDropOperation.Copy);
			}

			public override void PerformDrop (UIDropInteraction interaction, IUIDropSession session)
			{
				Console.WriteLine ($"PerformDrop ({interaction}, {session})");

				INSItemProviderReading objectType = new StringReader ();

				session.Completion (objectType, (objects) => {
					Console.WriteLine ($"PerformDropCompletion ({objects.Length})");

					dropView.BackgroundColor = UIColor.Green;
				});
			}

			#region HACK HACK HACK HACK

			class StringReader : NSObject, INSItemProviderReading
			{
				[Export ("readableTypeIdentifiersForItemProvider")]
				static NSArray ReadableTypeIdentifiers ()
				{
					Console.WriteLine ($"ReadableTypeIdentifiers");
					return NSArray.FromObjects (new NSString ("public.text"));
				}

				[Export ("objectWithItemProviderData:typeIdentifier:error:")]
				static NSObject ObjectWithItemProviderData (NSData data, NSString typeIdentifier, out NSError error)
				{
					Console.WriteLine ($"ObjectWithItemProviderData ({data}, {typeIdentifier})");
					error = null;
					return data.ToString (NSStringEncoding.UTF8);
				}
			}

			#endregion
		}
	}
}
