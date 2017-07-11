using System;

using ARKit;
using UIKit;
using SceneKit;

namespace Demo11.AwesomerReality
{
	public class AwesomerRealityViewController : UIViewController, IARSCNViewDelegate
	{
		ARSCNView sceneView = new ARSCNView ();

		public AwesomerRealityViewController ()
		{
			Title = "ARKit";

			View = sceneView;

			sceneView.Delegate = new Del ();
			sceneView.ShowsStatistics = true;
			sceneView.AutomaticallyUpdatesLighting = true;

			sceneView.Scene = SCNScene.Create ();
			var root = sceneView.Scene.RootNode;

			var cameraNode = SCNNode.Create ();
			cameraNode.Camera = SCNCamera.Create ();
			root.AddChildNode (cameraNode);

			var lightNode = SCNNode.Create ();
			cameraNode.Light = SCNLight.Create ();
			root.AddChildNode (lightNode);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			var config = new ARWorldTrackingSessionConfiguration {
				PlaneDetection = ARPlaneDetection.Horizontal,
				WorldAlignment = ARWorldAlignment.Gravity,
				LightEstimationEnabled = true,
			};

			sceneView.Session.Run (config);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			sceneView.Session.Pause ();
		}

		class Del : ARSCNViewDelegate
		{
			public override SCNNode GetNode (ISCNSceneRenderer renderer, ARAnchor anchor)
			{
				Console.WriteLine ($"GetNode ({renderer}, {anchor})");

				var node = SCNNode.Create ();

				node.Add (MakeBox (anchor));

				return node;
			}

			SCNNode MakeBox (ARAnchor achor)
			{
				Console.WriteLine ($"MakePlane ({achor})");

				var geometry = SCNBox.Create (0.1f, 0.1f, 0.1f, 0);
				geometry.FirstMaterial.Diffuse.ContentColor = UIColor.Yellow;

				var gnode = SCNNode.FromGeometry (geometry);
				return gnode;
			}

			SCNNode MakePlane (ARAnchor anchor)
			{
				var planeAchor = anchor as ARPlaneAnchor;
				if (planeAchor == null)
					return SCNNode.Create ();

				Console.WriteLine ($"MakePlane ({planeAchor})");

				var geometry = SCNPlane.Create (planeAchor.Extent.X, planeAchor.Extent.Z);
				geometry.FirstMaterial.Diffuse.ContentColor = UIColor.Green;

				var gnode = SCNNode.FromGeometry (geometry);
				gnode.Position = new SCNVector3 (planeAchor.Center.X, 0, planeAchor.Center.Z);
				//gnode.Transform = SCNMatrix4.CreateFromAxisAngle (SCNVector3.UnitX, (float)(Math.PI / 2));

				return gnode;
			}
		}
	}
}
