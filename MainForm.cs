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
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;

namespace FlyingToasters
{
	/// <summary>
	/// Draws the toasters.
	/// </summary>
	public partial class MainForm : Form
	{
		[DllImport("user32.dll")]
		static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
		
		[DllImport("user32.dll")]
		static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
		
		[DllImport("user32.dll", SetLastError = true)]
		static extern int GetWindowLong(IntPtr hWnd, int nIndex);
		
		[DllImport("user32.dll")]
		static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);
		

		private Random rnd_i = new Random();
		private const int NUM_TOASTERS = 10;
		private const int NUM_TOASTS = 5;
		private bool isCurrentlyAnimating_i = false;		
		private List<FlyingObject> objects_i = new List<FlyingObject>();
		private float fScreenScalingFactor_i = 1.0F;
		private Rectangle virtualScreenBounds_i;
		private Boolean isPreviewMode_i = false;
		
		
		
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
			//  Make our form fill ALL of the available screens
			//
			Rectangle totalSize = GetAllScreensBounds();
			this.Bounds = totalSize;
			
			SetVirtualScreenBounds();
			
    		Cursor.Hide();    		
			CreateFlyingObjects();
		}
		
		
		
		
		/// <summary>
		/// Alternative constructor that allows us to embed the screensaver in a preview
		/// window.  Thanks to http://www.codeproject.com/Articles/31376/Making-a-C-screensaver
		/// for the tips.
		/// </summary>
		/// <param name="hParentWnd_p">Window handle of the preview window</param>
		public MainForm(IntPtr hParentWnd_p)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			isPreviewMode_i = true;
			
			//
			//  Set the preview window as the parent of this one
			//
			SetParent(this.Handle, hParentWnd_p);
			
			//
			//  Tell Windows that our form is a child window; this means that we'll
			//  be closed automatically whenever the parent window closes.
			//
			SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

			//
			//  Set the window size to be the same as that of the parent window
			//
			Rectangle parentBounds;
		    GetClientRect(hParentWnd_p, out parentBounds);
		    this.Size = parentBounds.Size;
		    this.Location = new Point(0,0);
		    
		    fScreenScalingFactor_i = 10.0F;
		    SetVirtualScreenBounds();
		    
			this.SetStyle(ControlStyles.UserPaint, true);
    		this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
    		this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        		
    		Cursor.Hide();    		
			CreateFlyingObjects();
		}
		
		
		
		/// <summary>
		/// Mark it as a WS_EX_TOOLWINDOW window, so the icon doesn't show up in Alt Tab
		/// </summary>
		protected override CreateParams CreateParams 
		{
			get 
			{ 
				CreateParams originalParams = base.CreateParams;
				
				if (isPreviewMode_i)
				{
					originalParams.ExStyle |= 0x08000000; // WS_EX_NOACTIVATE
				}
				
				return originalParams;
			}
		} 
	
		
		
		/// <summary>
		/// Creates a dummy screen rectangle that, in preview mode at least, is 
		/// a scaled up version of the real window.  
		/// </summary>
		private void SetVirtualScreenBounds()
		{
			virtualScreenBounds_i = new Rectangle(this.Left,
			                                      this.Top,
												  (int) (this.Width * fScreenScalingFactor_i),
												  (int) (this.Height * fScreenScalingFactor_i));
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
			//  Add some toasters and toast
			//
			for (int i=0; i < NUM_TOASTERS; i++)
			{
				int iSpeed = rnd_i.Next(1,10);
				Bitmap gif = GetBitmapResource("toaster.gif");
				Point pos = GetRandomStartingPosition();
				FlyingObject o = new FlyingObject(gif, virtualScreenBounds_i, pos.X, pos.Y, iSpeed);
				objects_i.Add(o);
			}
			
			for (int i=0; i < NUM_TOASTS; i++)
			{
				int iSpeed = rnd_i.Next(1,10);
				Bitmap gif = GetBitmapResource("toast.gif");
				Point pos = GetRandomStartingPosition();
				FlyingObject o = new FlyingObject(gif, virtualScreenBounds_i, pos.X, pos.Y, iSpeed);
				objects_i.Add(o);
				
			}
		}
		
		
		
		
		private Point GetRandomStartingPosition()
		{
			return new Point(rnd_i.Next(0,virtualScreenBounds_i.Width + virtualScreenBounds_i.Height), rnd_i.Next(0,virtualScreenBounds_i.Height + 100));	
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
		
		
		
		
		/// <summary>
		/// Triggered by the Invalidate() method, this is where we draw the toasters
		/// and toast, and also trigger another 'AnimateImage' cycle.
		/// </summary>
		void MainFormPaint(object sender, PaintEventArgs e)
		{			
			AnimateImage();
			
			ImageAnimator.UpdateFrames();
			
			float fImageScalingFactor = 2 / fScreenScalingFactor_i;
			
			if (isPreviewMode_i)
			{	
				//
				//  Scale down using a high quality interpolation, so you can make 
				//  out what the tiny objects are.
				//
				e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			}
			else
			{
				//
				//  Scale the images up using retro chunky interpolation, not this
				//  fancy new bicubic nonsense.
				//
				e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			}
			
			foreach (FlyingObject o in objects_i)
			{
				//e.Graphics.DrawImage(o.Gif, o.Left, o.Top, o.Gif.Width*SCALE, o.Gif.Height*SCALE);
				e.Graphics.DrawImage(o.Gif, 
				                     o.Left / fScreenScalingFactor_i, 
				                     o.Top / fScreenScalingFactor_i, 
				                     o.Gif.Width*fImageScalingFactor, 
				                     o.Gif.Height*fImageScalingFactor);
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
