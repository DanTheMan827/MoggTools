using System;
using System.Windows.Forms;

namespace FolderSelect
{
	public class FolderSelectDialog
	{
		private OpenFileDialog ofd;

		public string FileName
		{
			get
			{
				return this.ofd.FileName;
			}
		}

		public string InitialDirectory
		{
			get
			{
				return this.ofd.InitialDirectory;
			}
			set
			{
				this.ofd.InitialDirectory = (value == null || value.Length == 0 ? Environment.CurrentDirectory : value);
			}
		}

		public string Title
		{
			get
			{
				return this.ofd.Title;
			}
			set
			{
				this.ofd.Title = (value == null ? "Select a folder" : value);
			}
		}

		public FolderSelectDialog()
		{
			this.ofd = new OpenFileDialog()
			{
				Filter = "Folders|\n",
				AddExtension = false,
				CheckFileExists = false,
				DereferenceLinks = true,
				Multiselect = false
			};
		}

		public bool ShowDialog()
		{
			return this.ShowDialog(IntPtr.Zero);
		}

		public bool ShowDialog(IntPtr hWndOwner)
		{
			bool flag = false;
			if (Environment.OSVersion.Version.Major < 6)
			{
				FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
				{
					Description = this.Title,
					SelectedPath = this.InitialDirectory,
					ShowNewFolderButton = false
				};
				if (folderBrowserDialog.ShowDialog(new WindowWrapper(hWndOwner)) != DialogResult.OK)
				{
					return false;
				}
				this.ofd.FileName = folderBrowserDialog.SelectedPath;
				flag = true;
			}
			else
			{
				Reflector reflector = new Reflector("System.Windows.Forms");
				uint num = 0;
				Type type = reflector.GetType("FileDialogNative.IFileDialog");
				object obj = reflector.Call(this.ofd, "CreateVistaDialog", new object[0]);
				reflector.Call(this.ofd, "OnBeforeVistaDialog", new object[] { obj });
				uint @enum = (uint)reflector.CallAs(typeof(FileDialog), this.ofd, "GetOptions", new object[0]);
				@enum |= (uint)reflector.GetEnum("FileDialogNative.FOS", "FOS_PICKFOLDERS");
				reflector.CallAs(type, obj, "SetOptions", new object[] { @enum });
				object obj1 = reflector.New("FileDialog.VistaDialogEvents", new object[] { this.ofd });
				object[] objArray = new object[] { obj1, num };
				reflector.CallAs2(type, obj, "Advise", objArray);
				num = (uint)objArray[1];
				try
				{
					int num1 = (int)reflector.CallAs(type, obj, "Show", new object[] { hWndOwner });
					flag = num1 == 0;
				}
				finally
				{
					reflector.CallAs(type, obj, "Unadvise", new object[] { num });
					GC.KeepAlive(obj1);
				}
			}
			return flag;
		}
	}
}