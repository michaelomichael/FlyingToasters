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
		
		
		private Bitmap GetBitmapResource(string sResourceName_p)
		{
			System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
	        Stream myStream = myAssembly.GetManifestResourceStream(sResourceName_p);
			Bitmap gif = new Bitmap(myStream);
			return gif;
		}
		
		
		private void CreateFlyingObjects()
		{		
			Rectangle totalSize = GetAllScreensBounds();
			
			this.Bounds = totalSize;
			
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
		
		void OnFrameChanged(object sender, EventArgs e)
		{
			this.Invalidate();  // Necessary, given that we're moving them on a timer anyway?
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			foreach (FlyingObject o in objects_i)
			{
				o.Move();
			}
			
			this.Invalidate();
		}
		
		void MainFormPaint(object sender, PaintEventArgs e)
		{
			const int SCALE = 1;
			
			AnimateImage();
			
			ImageAnimator.UpdateFrames();
			
			foreach (FlyingObject o in objects_i)
			{
				e.Graphics.DrawImage(o.Gif, o.Left, o.Top, o.Gif.Width*SCALE, o.Gif.Height*SCALE);
			}
		}
	}
}
