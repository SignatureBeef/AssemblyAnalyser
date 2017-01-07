using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;

namespace Resource.Test
{
	public class FlaggedClassAttribute : Attribute
	{

	}
	public class FlaggedMethodAttribute : Attribute
	{

	}

	[FlaggedClass]
	public class Class1
    {
		[FlaggedMethod]
		public void Proc()
		{
			Process.Start("asd");
		}

		public void N()
		{
			var wc = new WebClient();
			wc.DownloadFile("", "");
		}

		[DllImport("User32.dll")]
		public static extern int SetForegroundWindow(IntPtr point);
	}
}
