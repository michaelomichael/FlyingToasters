/*
 * Created by SharpDevelop.
 * User: Michael
 * Date: 04/11/2015
 * Time: 21:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FlyingToasters
{
	/// <summary>
	/// Description of Toaster.
	/// </summary>
	public partial class Toaster : UserControl
	{
		private const int MAX_TOASTER_IMAGES = 4;
		private int iImageIndex_i = 0;
		private int iWingsDirection_i = 1;
		private int iSpeed_i;
		private ImageList images_i;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="images_p">Should have 4 images</param>
		/// <param name="iSpeed_p">1 to 10</param>
		public Toaster(ImageList images_p, int iSpeed_p)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			images_i = images_p;
			iSpeed_i = iSpeed_p;
			timer1.Interval = (500 / iSpeed_p);
			timer1.Enabled = true;
		}
		
		
		public int Speed
		{
			get { return iSpeed_i; }
		}
		
		
		
		private void Timer1Tick(object sender, EventArgs e)
		{
			AnimateWings();
			Fly();
		}
		
		
		private void AnimateWings()
		{
			iImageIndex_i = iImageIndex_i + iWingsDirection_i;
			
			if (iImageIndex_i < 0)
			{
				iImageIndex_i = 1;
				iWingsDirection_i = -iWingsDirection_i;
			}
			else if (iImageIndex_i >= MAX_TOASTER_IMAGES)
			{
				iImageIndex_i = MAX_TOASTER_IMAGES - 2;
				iWingsDirection_i = -iWingsDirection_i;
			}
		
			this.BackgroundImage = images_i.Images[iImageIndex_i];
		}
		
		
		
		private void Fly()
		{
			float fPixelsPerTick = 1.5F;
			this.Left -= (int) (iSpeed_i * fPixelsPerTick);
			this.Top += (int) (iSpeed_i * fPixelsPerTick);
			
			if (this.Left < -100  ||  this.Top > this.Parent.Height)
			{
				// Reset
				int iDelta = Math.Max(this.Parent.Height, this.Parent.Width);
				this.Left += iDelta;
				this.Top -= iDelta;
			}
		}
	}
}
