/*
 * Created by SharpDevelop.
 * User: Michael
 * Date: 04/11/2015
 * Time: 21:05
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;

namespace FlyingToasters
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			
			if (args.Length > 0)
			{
				//MessageBox.Show("Args are " + args[0]);
				if (args[0].StartsWith("/c"))
				{
					//
					// Show configuration dialog
					//
					Application.Run(new ConfigForm());
					return;
				}
				else if (args[0].Equals("/p"))
				{
					//
					// Show a preview.
					//
					Application.Run(new MainForm(new IntPtr(long.Parse(args[1]))));
					return;
				}
			}
			
			//
			//  In all other cases (including the "/s" argument), just show full screen.
			//
			Application.Run(new MainForm());
		}
		
	}
}
