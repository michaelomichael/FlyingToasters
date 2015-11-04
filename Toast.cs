/*
 * Created by SharpDevelop.
 * User: Michael
 * Date: 04/11/2015
 * Time: 22:03
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
	/// Description of Toast.
	/// </summary>
	public partial class Toast : FlyingObject
	{
		
		public Toast() 
		{
			
		}
		
		public Toast(int iSpeed_p) : base(iSpeed_p)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
		}
	}
}
