/*
 * Created by SharpDevelop.
 * User: Michael
 * Date: 04/11/2015
 * Time: 21:56
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
	/// Description of FlyingObject.
	/// </summary>
	public partial class FlyingObject : UserControl
	{
		protected int iSpeed_i;
		
		public FlyingObject() : this(0)
		{
			
		}
		
		public FlyingObject(int iSpeed_p)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			iSpeed_i = iSpeed_p;
			
			if (iSpeed_i > 0)
			{
				timer1.Interval = 50 + (200 / iSpeed_p);
				timer1.Enabled = true;
			}
		}
		
		
		
		public int Speed
		{
			get { return iSpeed_i; }
		}
		
		
		
		
		protected virtual void Animate()
		{
			Fly();
		}
		
		
		
		protected void Fly()
		{
			
			int iPixelsPerTickX = (int) (iSpeed_i * 1F) + 0;
			int iPixelsPerTickY = (int) (iSpeed_i * 0.5F) + 0;
			
			this.Left -= iPixelsPerTickX;
			this.Top += iPixelsPerTickY;
			
			if (this.Left < -100  ||  this.Top > this.Parent.Height)
			{
				// Reset
				int iDelta = Math.Max(this.Parent.Height, this.Parent.Width);
				this.Left += iDelta;
				this.Top -= iDelta;
			}
		}
		
		
		
		private void Timer1Tick(object sender, EventArgs e)
		{
			Animate();
		}
	}
}
