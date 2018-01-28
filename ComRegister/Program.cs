using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace ComRegister
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				var reg = new ComRegister("GoogleAuth");
				if (reg.Register()== 1){
					Console.WriteLine("安装"+reg.DllFileName+"成功");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
			}
			finally
			{
				Console.ReadLine();
			}
		}

	}
}
