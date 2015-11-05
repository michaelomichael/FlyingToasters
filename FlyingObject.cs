/*
 * Created by SharpDevelop.
 * User: Michael
 * Date: 04/11/2015
 * Time: 23:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Diagnostics;

namespace FlyingToasters
{
	/// <summary>
	/// Description of FlyingObject.
	/// </summary>
	public class FlyingObject
	{
		private Bitmap gif_i;
		private int iLeft_i;
		private int iTop_i;
		private int iSpeed_i;
		private Rectangle screenBounds_i;
		
		
		
		public FlyingObject(Bitmap gif_p, Rectangle screenBounds_p, int iInitialLeft_p, int iInitialTop_p, int iSpeed_p)
		{
			gif_i = gif_p;
			iLeft_i = iInitialLeft_p;
			iTop_i = iInitialTop_p;
			iSpeed_i = iSpeed_p;
			screenBounds_i = screenBounds_p;
		}
		
		
		public void Move()
		{
			float fVerticalMovementProportion = 0.5F;
			
			int iPixelsPerTickX = (int) (((iSpeed_i+1) * 0.75F) + 0);
			int iPixelsPerTickY = (int) (iPixelsPerTickX * fVerticalMovementProportion);
			
			iLeft_i -= iPixelsPerTickX;
			iTop_i += iPixelsPerTickY;
			
			if (iLeft_i < (screenBounds_i.Left - 100)  ||  this.Top > screenBounds_i.Bottom)
			{
				// Reset
				Debug.WriteLine("Hello, resetting; Current (" + iLeft_i + "," + iTop_i + "), bounds = " + screenBounds_i);
				int iDelta = Math.Max(screenBounds_i.Height, screenBounds_i.Width);
				iLeft_i += iDelta;
				iTop_i -= (int) (iDelta * fVerticalMovementProportion);
			}
		}
		
		public Bitmap Gif {
			get { return gif_i; }
		}
		
		public int Left {
			get { return iLeft_i; }
		}

		public int Speed {
			get { return iSpeed_i; }
		}

		public int Top {
			get { return iTop_i; }
		}
	}
}
