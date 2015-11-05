/*
 * Created by SharpDevelop.
 * User: Michael
 * Date: 04/11/2015
 * Time: 21:05
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace FlyingToasters
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private Random rnd_i = new Random();
		private const int NUM_TOASTERS = 10;
		private const int NUM_TOASTS = 5;
		private bool isCurrentlyAnimating_i = false;		
		private List<FlyingObject> objects_i = new List<FlyingObject>();
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			
			this.SetStyle(ControlStyles.UserPaint, true);
    		this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
    		this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
    
    
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			CreateFlyingObjects();
		}
		
		
		
		private Rectangle GetAllScreensBounds()
		{
			Rectangle totalSize = Rectangle.Empty;
			 
			foreach (Screen s in Screen.AllScreens)
			{
				totalSize = System.Drawing.Rectangle.Union(totalSize, s.Bounds);
			}
			
			return totalSize;
		}
		
		
		
		
		//
		//  Loads a Bitmap out of this project/exe's resources.  To get them in there
		//  in the first place (in SharpDevelop, at least), right-click the project and
		//  "Add Existing Item" to add the image file to the project, then go to that
		//  image file's properties in SharpDevelop and set the "Build action" property
		//  to "Embedded Resource".
		//
		private Bitmap GetBitmapResource(string sResourceName_p)
		{
			System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
	        Stream myStream = myAssembly.GetManifestResourceStream(sResourceName_p);
			Bitmap gif = new Bitmap(myStream);
			return gif;
		}
		
		
		
		
		private void CreateFlyingObjects()
		{		
			//
			//  Make our form fill ALL of the available screens
			//
			Rectangle totalSize = GetAllScreensBounds();
			this.Bounds = totalSize;
			
			//
			//  Add some toasters and toast
			//
			for (int i=0; i < NUM_TOASTERS; i++)
			{
				int iSpeed = rnd_i.Next(1,10);
				Bitmap gif = GetBitmapResource("toaster.gif");
				Point pos = GetRandomStartingPosition();
				FlyingObject o = new FlyingObject(gif, totalSize, pos.X, pos.Y, iSpeed);
				objects_i.Add(o);
			}
			
			for (int i=0; i < NUM_TOASTS; i++)
			{
				int iSpeed = rnd_i.Next(1,10);
				Bitmap gif = GetBitmapResource("toast.gif");
				Point pos = GetRandomStartingPosition();
				FlyingObject o = new FlyingObject(gif, totalSize, pos.X, pos.Y, iSpeed);
				objects_i.Add(o);
				
			}
		}
		
		
		
		
		private Point GetRandomStartingPosition()
		{
			return new Point(rnd_i.Next(0,this.Width + this.Height), rnd_i.Next(0,this.Height + 100));	
		}
		
		
		
		void MainFormMouseMove(object sender, MouseEventArgs e)
		{
			
		}
		
		
		
		
		void MainFormMouseClick(object sender, MouseEventArgs e)
		{
			this.Close();
		}
		
		
		
		
		void MainFormKeyDown(object sender, KeyEventArgs e)
		{
			this.Close();
		}
		
		
		
		
		/// <summary>
		/// Instructs the ImageAnimator to check to see whether any of our toasters 
		/// need to move to the next frame.  
		/// </summary>
		private void AnimateImage()
		{
			if (! isCurrentlyAnimating_i)
			{
				foreach (FlyingObject o in objects_i)
				{
					ImageAnimator.Animate(o.Gif, new EventHandler(this.OnFrameChanged));
				}
				
				isCurrentlyAnimating_i = true;
			}
		}
		
		
		
		
		/// <summary>
		/// Fired when the ImageAnimator decides that a particular image/toaster needs to
		/// change.  It's probably not really required given that we call Invalidate() on
		/// a pretty fast background timer anyway.
		/// </summary>
		void OnFrameChanged(object sender, EventArgs e)
		{
			this.Invalidate(); 
		}
		
		
		
		
		void MainFormPaint(object sender, PaintEventArgs e)
		{			
			AnimateImage();
			
			ImageAnimator.UpdateFrames();
			
			//
			//  Scale the images up using retro chunky interpolation, not this
			//  fancy new bicubic nonsense.
			//
			const int SCALE = 2;			
			e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			
			foreach (FlyingObject o in objects_i)
			{
				e.Graphics.DrawImage(o.Gif, o.Left, o.Top, o.Gif.Width*SCALE, o.Gif.Height*SCALE);
			}
		}
		
		
		
		
		void TmrTopMostTick(object sender, EventArgs e)
		{
			//
			//  Ensure that this window is definitely the topmost.  If I'm honest, this
			//  has just been added because one of my other projects 
			//  (https://github.com/michaelomichael/OfficeChromeFix) doesn't detect screensavers
			//  just yet, and so tries to add a kind of window chrome to the fullscreen window!
			//
			this.TopMost = true;
		}
		
		
		
		
		void TmrAnimationTick(object sender, EventArgs e)
		{
			foreach (FlyingObject o in objects_i)
			{
				o.Move();
			}
			
			this.Invalidate();	
		}
	}
}
