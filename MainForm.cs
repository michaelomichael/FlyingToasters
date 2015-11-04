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
using System.Windows.Forms;

namespace FlyingToasters
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private List<Toaster> toasters_i = new List<Toaster>();
		private Random rnd_i = new Random();
		private const int NUM_TOASTERS = 10;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			Screen s = Screen.FromControl(this);
			this.Width = s.Bounds.Width;
			this.Height = s.Bounds.Height;
			for (int i=0; i < NUM_TOASTERS; i++)
			{
				int iSpeed = rnd_i.Next(1,10);
				Toaster t = new Toaster(imageList1, iSpeed);
				t.Location = new Point(rnd_i.Next(0,this.Width + this.Height), rnd_i.Next(0,this.Height + 100));
				this.Controls.Add(t);
			}
		}
		
		void MainFormMouseMove(object sender, MouseEventArgs e)
		{
			
		}
		
		void MainFormMouseClick(object sender, MouseEventArgs e)
		{
			this.Close();
		}
	}
}
