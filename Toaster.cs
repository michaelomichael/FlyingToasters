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
	public partial class Toaster : FlyingObject
	{
		private const int MAX_TOASTER_IMAGES = 4;
		private int iImageIndex_i = 0;
		private int iWingsDirection_i = 1;
		private int iTicksSinceLastAnimation_i = 0;
		
		private ImageList images_i;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="images_p">Should have 4 images</param>
		/// <param name="iSpeed_p">1 to 10</param>
		public Toaster(ImageList images_p, int iSpeed_p) : base(iSpeed_p)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			images_i = images_p;
			images_i.TransparentColor = Color.Transparent;
		}
		
		
		
		
		
		protected override void Animate()
		{
			base.Animate();
			AnimateWings();
		}
		
		
		
		private void AnimateWings()
		{
			iTicksSinceLastAnimation_i = (iTicksSinceLastAnimation_i + 1) % 3;
			
			if (0 == iTicksSinceLastAnimation_i)
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
				this.BackColor = Color.Transparent;
			}			
		}
		
		
		
		
	}
}
