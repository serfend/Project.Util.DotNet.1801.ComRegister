using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;

namespace ComRegister
{
	class ComRegister
	{
		private bool beenAdim;
		private string dllFileName;
		public ComRegister()
		{
			beenAdim = IsAdministrator();
			CheckAdmin();
		}
		public ComRegister(string target) : this()
		{
			dllFileName = target;
		}
		public string DllFileName { get => dllFileName; set => dllFileName = value; }

		public void CheckAdmin()
		{
			if (!beenAdim)
			{
				throw new NoAdministratorException();
			}
		}
		public int Register()
		{
			CheckAdmin();
			string systemPath = @"C:\Windows\" + DllFileName + ".dll";
			if (System.IO.File.Exists(systemPath))
			{
				System.IO.File.Delete(systemPath);
			}
			if(!System.IO.File.Exists(DllFileName + ".dll"))
			{
				throw new SourceNotExistException(DllFileName + ".dll");
			}
			string newPath = @"C:\Windows\" + DllFileName;
			System.IO.File.Copy(DllFileName + ".dll", newPath + ".dll");
			string thisPath = Environment.CurrentDirectory;
			string NetFrameWorkPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319";
			if (!System.IO.Directory.Exists(NetFrameWorkPath))
			{
				throw new FrameWorkNotExistException();
			}
			NetFrameWorkPath += "\\";
			ShellExcute.ShellExcuteExe("Regasm.exe", newPath + ".dll" + @" /tlb:" + newPath + @".tlb /codebase", NetFrameWorkPath);
			return 1;
		}
		private  bool IsAdministrator()
		{
			WindowsIdentity identity = WindowsIdentity.GetCurrent();
			WindowsPrincipal principal = new WindowsPrincipal(identity);
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}
	}

	[Serializable]
	public class FrameWorkNotExistException : Exception
	{
		public FrameWorkNotExistException():this("未安装.NET 4.0或更高版本") { }
		public FrameWorkNotExistException(string message) : base(message) { }
		public FrameWorkNotExistException(string message, Exception inner) : base(message, inner) { }
		protected FrameWorkNotExistException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
	[Serializable]
	public class SourceNotExistException : Exception
	{
		private string fileName;
		public SourceNotExistException(string fileName):this(fileName,"资源文件不存在") { }
		public SourceNotExistException(string fileName,string message) : base(message) {
			this.FileName = fileName;
		}
		public SourceNotExistException(string fileName,string message, Exception inner) : base(message, inner) {
			this.FileName = fileName;
		}
		protected SourceNotExistException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

		public string FileName { get => fileName; set => fileName = value; }

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
	[Serializable]
	public class NoAdministratorException :Exception
	{
		public NoAdministratorException():this("未获得管理员授权") { }
		public NoAdministratorException(string message) : base(message) { }
		public NoAdministratorException(string message, Exception inner) : base(message, inner) { }
		protected NoAdministratorException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
