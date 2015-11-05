/*
 * Created by SharpDevelop.
 * User: Michael
 * Date: 05/11/2015
 * Time: 13:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlyingToasters
{
	/// <summary>
	/// Description of ConfigForm.
	/// </summary>
	public partial class ConfigForm : Form
	{
		public ConfigForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
		}
		
		void CmdOKClick(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
