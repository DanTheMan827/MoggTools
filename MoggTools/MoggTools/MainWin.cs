using DtxCS;
using DtxCS.DataTypes;
using FolderSelect;
using GameArchives;
using GameArchives.Ark;
using GameArchives.STFS;
using LibMoggCrypt;
using LibOrbisPkg.Util;
using MidiCS;
using MoggTools.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MoggTools
{
	internal class MainWin : Form
	{
		private AbstractPackage pkg;

		private static MainWin static_this;

		private SongListSorter sorter;

		private IContainer components;

		private StatusStrip statusStrip;

		private MenuStrip menuStrip;

		private ToolStripMenuItem fileToolStripMenuItem;

		private ToolStripMenuItem exitToolStripMenuItem;

		private ToolStripMenuItem toolsToolStripMenuItem;

		private ToolStripMenuItem helpToolStripMenuItem;

		private ToolStripMenuItem aboutToolStripMenuItem;

		private ToolStripMenuItem decryptmoggToolStripMenuItem;

		private ToolStripStatusLabel statusLabel;

		private Label label1;

		private TextBox logTextBox;

		private Label label2;

		private System.Windows.Forms.ContextMenuStrip songListContextMenu;

		private Button checkAllBtn;

		private Button invertCheckedBtn;

		private Button extractCheckedMoggButton;

		private Button decryptCheckedMoggButton;

		private ToolStripMenuItem optionsToolStripMenuItem;

		private ToolStripMenuItem renameToArtistSongName;

		private ToolStripMenuItem createRPPToolStripMenuItem;

		private Button createRppCheckedButton;

		private ToolStripProgressBar workerProgressBar;

		private ToolStripMenuItem batchToolStripMenuItem;

		private ToolStripMenuItem extractMoggsFromCONLIVEInFolderToolStripMenuItem;

		private ToolStripMenuItem createRPPsForAllCONLIVEInFolderToolStripMenuItem;

		private PictureBox pictureBox1;

		private PictureBox pictureBox2;

		private ToolStripMenuItem unloadCurrentPackageToolStripMenuItem;

		private ListView songListView;

		private ColumnHeader artistHeader;

		private ColumnHeader nameHeader;

		private ColumnHeader albumHeader;

		private ColumnHeader yearHeader;

		private ToolStripMenuItem openPackageToolStripMenuItem;

		private ColumnHeader lengthHeader;

		private Label songCountLabel;

		private Button uncheckAllBtn;

		private GroupBox withCheckedGroupBox;

		private ToolStripMenuItem saveTempoMapInRPP;

		private ToolStripMenuItem saveMIDIDataInRPP;

		private ToolStripMenuItem showTutorialSongs;

		private Button extractCheckedDtasBtn;

		private Button extractCheckedMidisBtn;

		private ToolStripMenuItem decryptedToolStripMenuItem;

		private ToolStripMenuItem extractSelectedDTAsToolStripMenuItem;

		private ToolStripMenuItem extractSelectedmidsToolStripMenuItem;

		private ToolStripMenuItem encryptedToolStripMenuItem;

		private ToolStripMenuItem batchSaveDTAsToolStripMenuItem;

		private ToolStripMenuItem batchSavemidsToolStripMenuItem;

		private ToolStripMenuItem batchCreateRPPMoggsToolStripMenuItem;

		public static string AppName
		{
			get;
		}

		static MainWin()
		{
			MainWin.AppName = string.Concat(new object[] { "MoggTools v", Assembly.GetEntryAssembly().GetName().Version.Major, ".", Assembly.GetEntryAssembly().GetName().Version.Minor });
		}

		public MainWin()
		{
			this.InitializeComponent();
			this.sorter = new SongListSorter();
			this.songListView.ListViewItemSorter = this.sorter;
			this.songListView.FullRowSelect = true;
			this.songListView.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this.songListView, true, null);
			MainWin.static_this = this;
			this.renameToArtistSongName.Checked = Settings.Default.RenameToArtistSongName;
			this.saveMIDIDataInRPP.Checked = Settings.Default.SaveMidiDataInRPP;
			this.saveTempoMapInRPP.Checked = Settings.Default.SaveTempoMapInRPP;
			this.showTutorialSongs.Checked = Settings.Default.ShowTutorialSongs;
			this.unloadPackage();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(new AboutBox()).Show();
		}

		private void BatchCreateRPPMoggsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.doBatch((BatchWindow w, string a, string b) => w.batchExtractMoggsAndRPPs(a, b), "Select output directory...");
		}

		private void batchCreateRPPs_Click(object sender, EventArgs e)
		{
			this.doBatch((BatchWindow w, string a, string b) => w.batchExtractRPPs(a, b), "Select RPP directory...");
		}

		private void batchExtractMoggs_Click(object sender, EventArgs e)
		{
			this.doBatch((BatchWindow w, string a, string b) => w.batchExtractMoggs(a, b), "Select mogg output directory...");
		}

		private void batchSaveDTAs_Click(object sender, EventArgs e)
		{
			this.doBatch((BatchWindow w, string a, string b) => w.batchSaveDTAs(a, b), "Select dta output directory...");
		}

		private void batchSavemMids_Click(object sender, EventArgs e)
		{
			this.doBatch((BatchWindow w, string a, string b) => w.batchExtractMids(a, b), "Select midi output directory...");
		}

		private void checkAllBtn_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < this.songListView.Items.Count; i++)
			{
				this.songListView.Items[i].Checked = true;
			}
		}

		private void createSelectedRPPs_Click(object sender, EventArgs e)
		{
			FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
			if (folderSelectDialog.ShowDialog())
			{
				this.DoForAllSelected((Song s) => {
					Utils.SaveRpp(s, folderSelectDialog.FileName);
					this.LogLn(string.Concat("Saved ", s.GetFileName(), ".rpp"));
				});
			}
		}

		private void DecryptAndExtract(Song s, string path)
		{
			string str = string.Concat(s.GetFileName(), ".mogg");
			switch (Utils.SaveMogg(this.pkg.GetFile(string.Concat(s.Path, ".mogg")).GetStream(), Path.Combine(path, str), true))
			{
				case MoggCryptResult.ERR_DECRYPT_FAILED:
				{
					this.LogLn(string.Concat("Warning: Could not decrypt ", str, "."));
					return;
				}
				case MoggCryptResult.ERR_ALREADY_DECRYPTED:
				{
					this.LogLn(string.Concat("Saved ", str, " (not encrypted)"));
					return;
				}
				case MoggCryptResult.ERR_UNSUPPORTED_ENCRYPTION:
				{
					this.LogLn(string.Concat("Warning: Encryption for ", str, " not supported."));
					return;
				}
				case MoggCryptResult.SUCCESS:
				{
					this.LogLn(string.Concat("Saved ", str));
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private void decryptCheckedMoggsBtn_Click(object sender, EventArgs e)
		{
			FolderSelectDialog folderSelectDialog = new FolderSelectDialog()
			{
				Title = "Select output directory for extracted moggs"
			};
			if (folderSelectDialog.ShowDialog())
			{
				this.DoForAllChecked((Song s) => this.DecryptAndExtract(s, folderSelectDialog.FileName));
			}
		}

		private void decryptmoggToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				Title = "Select .mogg file...",
				Filter = "mogg files (*.mogg)|*.mogg",
				Multiselect = false
			};
			if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				this.statusLabel.Text = "Decrypting mogg...";
				this.LogLn(string.Concat("Loading ", openFileDialog.FileName, "..."));
				this.Refresh();
				byte[] numArray = File.ReadAllBytes(openFileDialog.FileName);
				this.LogLn(string.Concat(new object[] { "Decrypting ", openFileDialog.FileName, "... (", (int)numArray.Length, " bytes)" }));
				MoggCryptResult moggCryptResult = MoggCrypt.nativeDecrypt(numArray);
				MoggCryptResult moggCryptResult1 = moggCryptResult;
				if (moggCryptResult == MoggCryptResult.SUCCESS)
				{
					this.LogLn(string.Concat("Decrypted ", openFileDialog.FileName));
					SaveFileDialog saveFileDialog = new SaveFileDialog()
					{
						Title = "Select output filename...",
						Filter = "mogg files (*.mogg)|*.mogg"
					};
					if (saveFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					{
						File.WriteAllBytes(saveFileDialog.FileName, numArray);
						this.LogLn(string.Concat("Wrote decrypted mogg to ", saveFileDialog.FileName));
					}
				}
				else if (moggCryptResult1 == MoggCryptResult.ERR_ALREADY_DECRYPTED)
				{
					this.LogLn("Error decrypting mogg: already decrypted.");
				}
				else if (moggCryptResult1 == MoggCryptResult.ERR_UNSUPPORTED_ENCRYPTION)
				{
					this.LogLn("Error decrypting mogg: unsupported encryption scheme.");
				}
				else if (moggCryptResult1 == MoggCryptResult.ERR_DECRYPT_FAILED)
				{
					this.LogLn("Error decrypting mogg: supported encryption scheme, but decrypted data was wrong.");
				}
			}
			this.statusLabel.Text = "Ready";
			this.Refresh();
		}

		private void decryptSelectedMoggs_Click(object sender, EventArgs e)
		{
			FolderSelectDialog folderSelectDialog = new FolderSelectDialog()
			{
				Title = "Select output directory for extracted moggs."
			};
			if (folderSelectDialog.ShowDialog())
			{
				this.DoForAllSelected((Song s) => this.DecryptAndExtract(s, folderSelectDialog.FileName));
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void doBatch(Action<BatchWindow, string, string> batchAction, string outPrompt)
		{
			FolderSelectDialog folderSelectDialog = new FolderSelectDialog()
			{
				Title = "Select content file directory..."
			};
			FolderSelectDialog folderSelectDialog1 = new FolderSelectDialog()
			{
				Title = outPrompt
			};
			if (folderSelectDialog.ShowDialog() && folderSelectDialog1.ShowDialog())
			{
				BatchWindow batchWindow = new BatchWindow();
				batchWindow.Show();
				batchAction(batchWindow, folderSelectDialog.FileName, folderSelectDialog1.FileName);
			}
		}

		private void DoForAllChecked(Action<Song> work)
		{
			foreach (ListViewItem checkedItem in this.songListView.CheckedItems)
			{
				work(checkedItem.Tag as Song);
			}
		}

		private void DoForAllSelected(Action<Song> work)
		{
			foreach (ListViewItem selectedItem in this.songListView.SelectedItems)
			{
				work(selectedItem.Tag as Song);
			}
		}

		private void EnableAll()
		{
			this.songListView.Enabled = true;
			this.createRppCheckedButton.Enabled = true;
			this.extractCheckedMoggButton.Enabled = true;
			this.decryptCheckedMoggButton.Enabled = true;
			this.extractCheckedMidisBtn.Enabled = true;
			this.extractCheckedDtasBtn.Enabled = true;
			this.unloadCurrentPackageToolStripMenuItem.Enabled = true;
			this.checkAllBtn.Enabled = true;
			this.uncheckAllBtn.Enabled = true;
			this.invertCheckedBtn.Enabled = true;
			this.Text = string.Concat(MainWin.AppName, " - ", this.pkg.FileName);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void extractCheckedDtasBtn_Click(object sender, EventArgs e)
		{
			FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
			if (folderSelectDialog.ShowDialog())
			{
				this.DoForAllChecked((Song s) => {
					Utils.SaveDTA(s, folderSelectDialog.FileName);
					this.LogLn(string.Concat("Saved ", s.GetFileName(), ".dta"));
				});
			}
		}

		private void extractCheckedMidisBtn_Click(object sender, EventArgs e)
		{
			FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
			if (folderSelectDialog.ShowDialog())
			{
				this.DoForAllChecked((Song s) => {
					Util.ExtractTo(this.pkg.GetFile(string.Concat(s.Path, ".mid")), Path.Combine(folderSelectDialog.FileName, string.Concat(s.GetFileName(), ".mid")));
					this.LogLn(string.Concat("Saved ", s.GetFileName(), ".mid"));
				});
			}
		}

		private void extractCheckedMoggsBtn_Click(object sender, EventArgs e)
		{
			FolderSelectDialog folderSelectDialog = new FolderSelectDialog()
			{
				Title = "Select output directory for extracted moggs"
			};
			if (folderSelectDialog.ShowDialog())
			{
				this.DoForAllChecked((Song s) => {
					string str = string.Concat(s.GetFileName(), ".mogg");
					Utils.SaveMogg(this.pkg.GetFile(string.Concat(s.Path, ".mogg")).GetStream(), Path.Combine(folderSelectDialog.FileName, str), false);
					MainWin.LogLn_(string.Concat("Saved ", str));
				});
			}
		}

		private void extractCheckedRPPsBtn_Click(object sender, EventArgs e)
		{
			FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
			if (folderSelectDialog.ShowDialog())
			{
				this.DoForAllChecked((Song s) => {
					Utils.SaveRpp(s, folderSelectDialog.FileName);
					this.LogLn(string.Concat("Saved ", s.GetFileName(), ".rpp"));
				});
			}
		}

		private void extractSelectedDTAs_Click(object sender, EventArgs e)
		{
			FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
			if (folderSelectDialog.ShowDialog())
			{
				this.DoForAllSelected((Song s) => {
					Utils.SaveDTA(s, folderSelectDialog.FileName);
					this.LogLn(string.Concat("Saved ", s.GetFileName(), ".dta"));
				});
			}
		}

		private void extractSelectedmids_Click(object sender, EventArgs e)
		{
			FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
			if (folderSelectDialog.ShowDialog())
			{
				this.DoForAllSelected((Song s) => {
					Util.ExtractTo(this.pkg.GetFile(string.Concat(s.Path, ".mid")), Path.Combine(folderSelectDialog.FileName, string.Concat(s.GetFileName(), ".mid")));
					this.LogLn(string.Concat("Saved ", s.GetFileName(), ".mid"));
				});
			}
		}

		private void extractSelectedMoggs_Click(object sender, EventArgs e)
		{
			FolderSelectDialog folderSelectDialog = new FolderSelectDialog()
			{
				Title = "Select output directory for extracted moggs."
			};
			if (folderSelectDialog.ShowDialog())
			{
				this.DoForAllSelected((Song s) => {
					string str = string.Concat(s.GetFileName(), ".mogg");
					Utils.SaveMogg(this.pkg.GetFile(string.Concat(s.Path, ".mogg")).GetStream(), Path.Combine(folderSelectDialog.FileName, str), false);
					MainWin.LogLn_(string.Concat("Saved ", str));
				});
			}
		}

		private void FileDragDrop(object sender, DragEventArgs e)
		{
			if ((int)((string[])e.Data.GetData(DataFormats.FileDrop)).Length > 1)
			{
				this.LogLn("Please only drop one file at a time. Support for multiple drops may be added in the future.");
				return;
			}
			string data = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
			this.LoadFile(data);
		}

		private void FileDragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.workerProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unloadCurrentPackageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractMoggsFromCONLIVEInFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchSavemidsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchSaveDTAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createRPPsForAllCONLIVEInFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchCreateRPPMoggsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptmoggToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToArtistSongName = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTempoMapInRPP = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMIDIDataInRPP = new System.Windows.Forms.ToolStripMenuItem();
            this.showTutorialSongs = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.songListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.decryptedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createRPPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractSelectedDTAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractSelectedmidsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encryptedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.checkAllBtn = new System.Windows.Forms.Button();
            this.invertCheckedBtn = new System.Windows.Forms.Button();
            this.extractCheckedMoggButton = new System.Windows.Forms.Button();
            this.decryptCheckedMoggButton = new System.Windows.Forms.Button();
            this.createRppCheckedButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.songListView = new System.Windows.Forms.ListView();
            this.nameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.artistHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.albumHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.yearHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lengthHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.songCountLabel = new System.Windows.Forms.Label();
            this.uncheckAllBtn = new System.Windows.Forms.Button();
            this.withCheckedGroupBox = new System.Windows.Forms.GroupBox();
            this.extractCheckedDtasBtn = new System.Windows.Forms.Button();
            this.extractCheckedMidisBtn = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.songListContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.withCheckedGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.workerProgressBar,
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 494);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(704, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // workerProgressBar
            // 
            this.workerProgressBar.Name = "workerProgressBar";
            this.workerProgressBar.Size = new System.Drawing.Size(100, 16);
            this.workerProgressBar.Step = 1;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Ready";
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.batchToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(704, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openPackageToolStripMenuItem,
            this.unloadCurrentPackageToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openPackageToolStripMenuItem
            // 
            this.openPackageToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.openPackageToolStripMenuItem.Name = "openPackageToolStripMenuItem";
            this.openPackageToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.openPackageToolStripMenuItem.Text = "&Open package...";
            this.openPackageToolStripMenuItem.Click += new System.EventHandler(this.openPackageToolStripMenuItem_Click);
            // 
            // unloadCurrentPackageToolStripMenuItem
            // 
            this.unloadCurrentPackageToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.unloadCurrentPackageToolStripMenuItem.Name = "unloadCurrentPackageToolStripMenuItem";
            this.unloadCurrentPackageToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.unloadCurrentPackageToolStripMenuItem.Text = "&Close package";
            this.unloadCurrentPackageToolStripMenuItem.Click += new System.EventHandler(this.unloadCurrentPackage_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // batchToolStripMenuItem
            // 
            this.batchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractMoggsFromCONLIVEInFolderToolStripMenuItem,
            this.batchSavemidsToolStripMenuItem,
            this.batchSaveDTAsToolStripMenuItem,
            this.createRPPsForAllCONLIVEInFolderToolStripMenuItem,
            this.batchCreateRPPMoggsToolStripMenuItem});
            this.batchToolStripMenuItem.Name = "batchToolStripMenuItem";
            this.batchToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.batchToolStripMenuItem.Text = "&Batch";
            // 
            // extractMoggsFromCONLIVEInFolderToolStripMenuItem
            // 
            this.extractMoggsFromCONLIVEInFolderToolStripMenuItem.Name = "extractMoggsFromCONLIVEInFolderToolStripMenuItem";
            this.extractMoggsFromCONLIVEInFolderToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
            this.extractMoggsFromCONLIVEInFolderToolStripMenuItem.Text = "Batch &extract moggs (decrypted)...";
            this.extractMoggsFromCONLIVEInFolderToolStripMenuItem.Click += new System.EventHandler(this.batchExtractMoggs_Click);
            // 
            // batchSavemidsToolStripMenuItem
            // 
            this.batchSavemidsToolStripMenuItem.Name = "batchSavemidsToolStripMenuItem";
            this.batchSavemidsToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
            this.batchSavemidsToolStripMenuItem.Text = "Batch extract .&mids...";
            this.batchSavemidsToolStripMenuItem.Click += new System.EventHandler(this.batchSavemMids_Click);
            // 
            // batchSaveDTAsToolStripMenuItem
            // 
            this.batchSaveDTAsToolStripMenuItem.Name = "batchSaveDTAsToolStripMenuItem";
            this.batchSaveDTAsToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
            this.batchSaveDTAsToolStripMenuItem.Text = "Batch save .&dtas...";
            this.batchSaveDTAsToolStripMenuItem.Click += new System.EventHandler(this.batchSaveDTAs_Click);
            // 
            // createRPPsForAllCONLIVEInFolderToolStripMenuItem
            // 
            this.createRPPsForAllCONLIVEInFolderToolStripMenuItem.Name = "createRPPsForAllCONLIVEInFolderToolStripMenuItem";
            this.createRPPsForAllCONLIVEInFolderToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
            this.createRPPsForAllCONLIVEInFolderToolStripMenuItem.Text = "Batch create &RPPs...";
            this.createRPPsForAllCONLIVEInFolderToolStripMenuItem.Click += new System.EventHandler(this.batchCreateRPPs_Click);
            // 
            // batchCreateRPPMoggsToolStripMenuItem
            // 
            this.batchCreateRPPMoggsToolStripMenuItem.Name = "batchCreateRPPMoggsToolStripMenuItem";
            this.batchCreateRPPMoggsToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
            this.batchCreateRPPMoggsToolStripMenuItem.Text = "Batch create RPP+Moggs...";
            this.batchCreateRPPMoggsToolStripMenuItem.Click += new System.EventHandler(this.BatchCreateRPPMoggsToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decryptmoggToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // decryptmoggToolStripMenuItem
            // 
            this.decryptmoggToolStripMenuItem.Name = "decryptmoggToolStripMenuItem";
            this.decryptmoggToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.decryptmoggToolStripMenuItem.Text = "&Decrypt single .mogg...";
            this.decryptmoggToolStripMenuItem.Click += new System.EventHandler(this.decryptmoggToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToArtistSongName,
            this.saveTempoMapInRPP,
            this.saveMIDIDataInRPP,
            this.showTutorialSongs});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // renameToArtistSongName
            // 
            this.renameToArtistSongName.Checked = true;
            this.renameToArtistSongName.CheckOnClick = true;
            this.renameToArtistSongName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.renameToArtistSongName.Name = "renameToArtistSongName";
            this.renameToArtistSongName.Size = new System.Drawing.Size(210, 22);
            this.renameToArtistSongName.Text = "Rename to \"Artist - Song\"";
            this.renameToArtistSongName.Click += new System.EventHandler(this.renameToArtistSongName_Click);
            // 
            // saveTempoMapInRPP
            // 
            this.saveTempoMapInRPP.Checked = true;
            this.saveTempoMapInRPP.CheckOnClick = true;
            this.saveTempoMapInRPP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saveTempoMapInRPP.Name = "saveTempoMapInRPP";
            this.saveTempoMapInRPP.Size = new System.Drawing.Size(210, 22);
            this.saveTempoMapInRPP.Text = "Save tempo map in RPP";
            this.saveTempoMapInRPP.Click += new System.EventHandler(this.saveTempoMapInRPP_Click);
            // 
            // saveMIDIDataInRPP
            // 
            this.saveMIDIDataInRPP.CheckOnClick = true;
            this.saveMIDIDataInRPP.Name = "saveMIDIDataInRPP";
            this.saveMIDIDataInRPP.Size = new System.Drawing.Size(210, 22);
            this.saveMIDIDataInRPP.Text = "Save MIDI data in RPP";
            this.saveMIDIDataInRPP.Click += new System.EventHandler(this.saveMIDIDataInRPP_Click);
            // 
            // showTutorialSongs
            // 
            this.showTutorialSongs.CheckOnClick = true;
            this.showTutorialSongs.Name = "showTutorialSongs";
            this.showTutorialSongs.Size = new System.Drawing.Size(210, 22);
            this.showTutorialSongs.Text = "Show tutorial songs";
            this.showTutorialSongs.Click += new System.EventHandler(this.showTutorialSongs_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 358);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Log:";
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Location = new System.Drawing.Point(12, 374);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(680, 119);
            this.logTextBox.TabIndex = 3;
            // 
            // songListContextMenu
            // 
            this.songListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decryptedToolStripMenuItem,
            this.createRPPToolStripMenuItem,
            this.extractSelectedDTAsToolStripMenuItem,
            this.extractSelectedmidsToolStripMenuItem,
            this.encryptedToolStripMenuItem});
            this.songListContextMenu.Name = "contextMenuStrip1";
            this.songListContextMenu.Size = new System.Drawing.Size(264, 114);
            // 
            // decryptedToolStripMenuItem
            // 
            this.decryptedToolStripMenuItem.Name = "decryptedToolStripMenuItem";
            this.decryptedToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.decryptedToolStripMenuItem.Text = "Extract selected .moggs (decrypted)";
            this.decryptedToolStripMenuItem.Click += new System.EventHandler(this.decryptSelectedMoggs_Click);
            // 
            // createRPPToolStripMenuItem
            // 
            this.createRPPToolStripMenuItem.Name = "createRPPToolStripMenuItem";
            this.createRPPToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.createRPPToolStripMenuItem.Text = "Create RPPs for selected";
            this.createRPPToolStripMenuItem.Click += new System.EventHandler(this.extractCheckedRPPsBtn_Click);
            // 
            // extractSelectedDTAsToolStripMenuItem
            // 
            this.extractSelectedDTAsToolStripMenuItem.Name = "extractSelectedDTAsToolStripMenuItem";
            this.extractSelectedDTAsToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.extractSelectedDTAsToolStripMenuItem.Text = "Extract selected .dtas";
            this.extractSelectedDTAsToolStripMenuItem.Click += new System.EventHandler(this.extractSelectedDTAs_Click);
            // 
            // extractSelectedmidsToolStripMenuItem
            // 
            this.extractSelectedmidsToolStripMenuItem.Name = "extractSelectedmidsToolStripMenuItem";
            this.extractSelectedmidsToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.extractSelectedmidsToolStripMenuItem.Text = "Extract selected .mids";
            this.extractSelectedmidsToolStripMenuItem.Click += new System.EventHandler(this.extractSelectedmids_Click);
            // 
            // encryptedToolStripMenuItem
            // 
            this.encryptedToolStripMenuItem.Name = "encryptedToolStripMenuItem";
            this.encryptedToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.encryptedToolStripMenuItem.Text = "Extract selected .moggs (as-is)";
            this.encryptedToolStripMenuItem.Click += new System.EventHandler(this.extractSelectedMoggs_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(160, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Songs in pack:";
            // 
            // checkAllBtn
            // 
            this.checkAllBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkAllBtn.Location = new System.Drawing.Point(163, 43);
            this.checkAllBtn.Name = "checkAllBtn";
            this.checkAllBtn.Size = new System.Drawing.Size(60, 23);
            this.checkAllBtn.TabIndex = 15;
            this.checkAllBtn.Text = "Check All";
            this.checkAllBtn.UseVisualStyleBackColor = true;
            this.checkAllBtn.Click += new System.EventHandler(this.checkAllBtn_Click);
            // 
            // invertCheckedBtn
            // 
            this.invertCheckedBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.invertCheckedBtn.Location = new System.Drawing.Point(163, 70);
            this.invertCheckedBtn.Name = "invertCheckedBtn";
            this.invertCheckedBtn.Size = new System.Drawing.Size(140, 23);
            this.invertCheckedBtn.TabIndex = 16;
            this.invertCheckedBtn.Text = "Invert Checked";
            this.invertCheckedBtn.UseVisualStyleBackColor = true;
            this.invertCheckedBtn.Click += new System.EventHandler(this.invertCheckedBtn_Click);
            // 
            // extractCheckedMoggButton
            // 
            this.extractCheckedMoggButton.Location = new System.Drawing.Point(100, 19);
            this.extractCheckedMoggButton.Name = "extractCheckedMoggButton";
            this.extractCheckedMoggButton.Size = new System.Drawing.Size(88, 23);
            this.extractCheckedMoggButton.TabIndex = 17;
            this.extractCheckedMoggButton.Text = "Extract .moggs";
            this.extractCheckedMoggButton.UseVisualStyleBackColor = true;
            this.extractCheckedMoggButton.Click += new System.EventHandler(this.extractCheckedMoggsBtn_Click);
            // 
            // decryptCheckedMoggButton
            // 
            this.decryptCheckedMoggButton.Location = new System.Drawing.Point(6, 46);
            this.decryptCheckedMoggButton.Name = "decryptCheckedMoggButton";
            this.decryptCheckedMoggButton.Size = new System.Drawing.Size(182, 23);
            this.decryptCheckedMoggButton.TabIndex = 18;
            this.decryptCheckedMoggButton.Text = "Decrypt and Extract .moggs";
            this.decryptCheckedMoggButton.UseVisualStyleBackColor = true;
            this.decryptCheckedMoggButton.Click += new System.EventHandler(this.decryptCheckedMoggsBtn_Click);
            // 
            // createRppCheckedButton
            // 
            this.createRppCheckedButton.Location = new System.Drawing.Point(6, 19);
            this.createRppCheckedButton.Name = "createRppCheckedButton";
            this.createRppCheckedButton.Size = new System.Drawing.Size(88, 23);
            this.createRppCheckedButton.TabIndex = 19;
            this.createRppCheckedButton.Text = "Create RPPs";
            this.createRppCheckedButton.UseVisualStyleBackColor = true;
            this.createRppCheckedButton.Click += new System.EventHandler(this.extractCheckedRPPsBtn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(12, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(66, 66);
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(88, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(66, 66);
            this.pictureBox2.TabIndex = 22;
            this.pictureBox2.TabStop = false;
            // 
            // songListView
            // 
            this.songListView.AllowColumnReorder = true;
            this.songListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.songListView.CheckBoxes = true;
            this.songListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameHeader,
            this.artistHeader,
            this.albumHeader,
            this.yearHeader,
            this.lengthHeader});
            this.songListView.HideSelection = false;
            this.songListView.Location = new System.Drawing.Point(12, 99);
            this.songListView.Name = "songListView";
            this.songListView.Size = new System.Drawing.Size(681, 256);
            this.songListView.TabIndex = 23;
            this.songListView.UseCompatibleStateImageBehavior = false;
            this.songListView.View = System.Windows.Forms.View.Details;
            this.songListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.songListView_ColumnClick);
            this.songListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.songListView_ItemSelectionChanged);
            this.songListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.songListView_Clicked);
            // 
            // nameHeader
            // 
            this.nameHeader.Text = "Name";
            this.nameHeader.Width = 212;
            // 
            // artistHeader
            // 
            this.artistHeader.Text = "Artist";
            this.artistHeader.Width = 192;
            // 
            // albumHeader
            // 
            this.albumHeader.Text = "Album";
            this.albumHeader.Width = 169;
            // 
            // yearHeader
            // 
            this.yearHeader.Text = "Year";
            this.yearHeader.Width = 37;
            // 
            // lengthHeader
            // 
            this.lengthHeader.Text = "Length";
            this.lengthHeader.Width = 46;
            // 
            // songCountLabel
            // 
            this.songCountLabel.AutoSize = true;
            this.songCountLabel.Location = new System.Drawing.Point(233, 27);
            this.songCountLabel.Name = "songCountLabel";
            this.songCountLabel.Size = new System.Drawing.Size(35, 13);
            this.songCountLabel.TabIndex = 24;
            this.songCountLabel.Text = "label3";
            // 
            // uncheckAllBtn
            // 
            this.uncheckAllBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uncheckAllBtn.Location = new System.Drawing.Point(229, 43);
            this.uncheckAllBtn.Name = "uncheckAllBtn";
            this.uncheckAllBtn.Size = new System.Drawing.Size(74, 23);
            this.uncheckAllBtn.TabIndex = 26;
            this.uncheckAllBtn.Text = "Uncheck All";
            this.uncheckAllBtn.UseVisualStyleBackColor = true;
            this.uncheckAllBtn.Click += new System.EventHandler(this.uncheckAllBtn_Click);
            // 
            // withCheckedGroupBox
            // 
            this.withCheckedGroupBox.Controls.Add(this.extractCheckedDtasBtn);
            this.withCheckedGroupBox.Controls.Add(this.extractCheckedMidisBtn);
            this.withCheckedGroupBox.Controls.Add(this.createRppCheckedButton);
            this.withCheckedGroupBox.Controls.Add(this.extractCheckedMoggButton);
            this.withCheckedGroupBox.Controls.Add(this.decryptCheckedMoggButton);
            this.withCheckedGroupBox.Location = new System.Drawing.Point(309, 24);
            this.withCheckedGroupBox.Name = "withCheckedGroupBox";
            this.withCheckedGroupBox.Size = new System.Drawing.Size(290, 72);
            this.withCheckedGroupBox.TabIndex = 27;
            this.withCheckedGroupBox.TabStop = false;
            this.withCheckedGroupBox.Text = "With Checked";
            // 
            // extractCheckedDtasBtn
            // 
            this.extractCheckedDtasBtn.Location = new System.Drawing.Point(194, 46);
            this.extractCheckedDtasBtn.Name = "extractCheckedDtasBtn";
            this.extractCheckedDtasBtn.Size = new System.Drawing.Size(88, 23);
            this.extractCheckedDtasBtn.TabIndex = 21;
            this.extractCheckedDtasBtn.Text = "Extract .dtas";
            this.extractCheckedDtasBtn.UseVisualStyleBackColor = true;
            this.extractCheckedDtasBtn.Click += new System.EventHandler(this.extractCheckedDtasBtn_Click);
            // 
            // extractCheckedMidisBtn
            // 
            this.extractCheckedMidisBtn.Location = new System.Drawing.Point(194, 19);
            this.extractCheckedMidisBtn.Name = "extractCheckedMidisBtn";
            this.extractCheckedMidisBtn.Size = new System.Drawing.Size(88, 23);
            this.extractCheckedMidisBtn.TabIndex = 20;
            this.extractCheckedMidisBtn.Text = "Extract .mids";
            this.extractCheckedMidisBtn.UseVisualStyleBackColor = true;
            this.extractCheckedMidisBtn.Click += new System.EventHandler(this.extractCheckedMidisBtn_Click);
            // 
            // MainWin
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 516);
            this.Controls.Add(this.withCheckedGroupBox);
            this.Controls.Add(this.uncheckAllBtn);
            this.Controls.Add(this.songCountLabel);
            this.Controls.Add(this.songListView);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.invertCheckedBtn);
            this.Controls.Add(this.checkAllBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(720, 480);
            this.Name = "MainWin";
            this.Text = "MoggTools by Maxton";
            this.Load += new System.EventHandler(this.MainWin_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FileDragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FileDragEnter);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.songListContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.withCheckedGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private void invertCheckedBtn_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < this.songListView.Items.Count; i++)
			{
				this.songListView.Items[i].Checked = !this.songListView.Items[i].Checked;
			}
		}

		private void LoadArk(IFile file)
		{
			IDirectory directory = null;
			DataArray dataArray;
			this.unloadPackage();
			this.LogLn(string.Concat("Loading ark package ", file.Name));
			try
			{
				this.pkg = PackageReader.ReadPackageFromFile(file);
				string[] strArrays = new string[] { "pc", "ps4" };
				int num = 0;
				while (num < (int)strArrays.Length)
				{
					string str = strArrays[num];
					if (!this.pkg.RootDirectory.TryGetDirectory(str, out directory))
					{
						num++;
					}
					else
					{
						try
						{
							this.PopulateListView(Utils.LoadSongsForge(directory, str));
							return;
						}
						catch (Exception exception1)
						{
							Exception exception = exception1;
							this.LogLn("Unable to load forge ark.");
							this.LogLn(exception.StackTrace);
							return;
						}
					}
				}
				if (!this.pkg.RootDirectory.TryGetDirectory("sce_sys", out directory))
				{
					try
					{
						using (Stream stream = this.pkg.GetFile("songs/gen/songs.dtb").GetStream())
						{
							dataArray = DTX.FromDtb(stream);
						}
						this.PopulateListView(Utils.LoadSongs(dataArray, this.pkg));
					}
					catch (FileNotFoundException fileNotFoundException)
					{
						this.LogLn("Unable to load ark; could not find songs.dtb");
					}
				}
				else
				{
					try
					{
						this.PopulateListView(Utils.LoadSongsForge(this.pkg.RootDirectory, "ps4"));
					}
					catch (Exception exception3)
					{
						Exception exception2 = exception3;
						this.LogLn("Unable to load PS4 DLC.");
						this.LogLn(exception2.StackTrace);
					}
				}
			}
			catch (FileNotFoundException fileNotFoundException1)
			{
				this.LogLn(string.Concat("Couldn't load ark; matching content file not found: ", fileNotFoundException1.FileName));
			}
			catch (Exception exception5)
			{
				Exception exception4 = exception5;
				this.LogLn(string.Concat("Got unexpected exception when trying to load ark package: ", exception4.Message));
				this.LogLn(exception4.StackTrace);
			}
		}

		private void LoadFile(string dropFile)
		{
			MoggCryptResult moggCryptResult;
			DataArray dataArray;
			IFile file = Util.LocalFile(dropFile);
			if (STFSPackage.IsSTFS(file) == PackageTestResult.YES)
			{
				this.LoadSTFS(file);
				return;
			}
			if (ArkPackage.IsArk(file) >= PackageTestResult.MAYBE || file.Name.EndsWith("dat"))
			{
				this.LoadArk(file);
				return;
			}
			if (Path.GetExtension(dropFile).ToLower() == ".mogg")
			{
				this.LogLn("Decrypting dropped mogg file...");
				try
				{
					SaveFileDialog saveFileDialog = new SaveFileDialog()
					{
						Title = "Choose output filename."
					};
					SaveFileDialog saveFileDialog1 = saveFileDialog;
					saveFileDialog1.Filter = string.Concat(saveFileDialog1.Filter, "Mogg Files (*.mogg)|*.mogg");
					saveFileDialog.InitialDirectory = Path.GetDirectoryName(dropFile);
					saveFileDialog.FileName = Path.GetFileName(dropFile);
					if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
					{
						this.LogLn("Decryption cancelled.");
					}
					else
					{
						using (Stream fileStream = new FileStream(dropFile, FileMode.Open))
						{
							moggCryptResult = Utils.SaveMogg(fileStream, saveFileDialog.FileName, true);
						}
						if (moggCryptResult == MoggCryptResult.ERR_DECRYPT_FAILED || moggCryptResult == MoggCryptResult.ERR_UNSUPPORTED_ENCRYPTION)
						{
							this.LogLn("Error decrypting dropped mogg.");
						}
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					this.LogLn(exception.Message);
					this.LogLn(exception.StackTrace);
				}
			}
			else if (Path.GetExtension(dropFile).ToLower() == ".dta")
			{
				try
				{
					using (FileStream fileStream1 = new FileStream(dropFile, FileMode.Open, FileAccess.Read))
					{
						//dataArray = DTX.FromPlainTextBytes(fileStream1.ReadBytes((long)((int)fileStream1.Length)));
						dataArray = DTX.FromPlainTextBytes(fileStream1.ReadBytes((int)fileStream1.Length));
					}
					Song[] song = new Song[dataArray.Count];
					for (int i = 0; i < dataArray.Count; i++)
					{
						song[i] = new Song(dataArray.Array(i), null);
					}
					FolderSelectDialog folderSelectDialog = new FolderSelectDialog()
					{
						Title = "Select output folder for RPPs.",
						InitialDirectory = Path.GetDirectoryName(dropFile)
					};
					if (folderSelectDialog.ShowDialog())
					{
						Song[] songArray = song;
						for (int j = 0; j < (int)songArray.Length; j++)
						{
							Utils.SaveRpp(songArray[j], folderSelectDialog.FileName);
						}
					}
				}
				catch (Exception exception2)
				{
					this.LogLn("Error making RPPs from dropped dta.");
				}
			}
			else if (Path.GetExtension(dropFile).ToLower() != ".mid")
			{
				this.LogLn("File was not an STFS archive, DTA file, Ark .hdr file or .mogg file.");
			}
			else
			{
				using (FileStream fileStream2 = new FileStream(dropFile, FileMode.Open, FileAccess.Read))
				{
					MidiFile midiFile = MidiFileReader.FromStream(fileStream2);
					this.LogLn(string.Concat("New midi file dropped, duration ", midiFile.Duration));
				}
			}
		}

		private void LoadSTFS(IFile file)
		{
			this.unloadPackage();
			this.pkg = STFSPackage.OpenFile(file);
			STFSPackage sTFSPackage = (STFSPackage)this.pkg;
			this.pictureBox1.Image = sTFSPackage.Thumbnail;
			this.pictureBox2.Image = sTFSPackage.TitleThumbnail;
			this.LogLn(string.Concat("Loading songs from '", this.pkg.FileName, "'..."));
			this.statusLabel.Text = "Loading...";
			try
			{
				DataArray dataArray = DTX.FromPlainTextBytes(this.pkg.GetFile("songs/songs.dta").GetBytes());
				try
				{
					this.PopulateListView(Utils.LoadSongs(dataArray, this.pkg));
				}
				catch (Exception exception)
				{
					this.LogLn(string.Concat("Error loading songs from pack: ", exception.Message));
				}
			}
			catch (Exception exception1)
			{
				this.LogLn("Error loading songs.dta!");
			}
		}

		public void Log(string txt)
		{
			this.logTextBox.Invoke(new MethodInvoker(() => this.logTextBox.AppendText(txt)));
		}

		public static void Log_(string txt)
		{
			MainWin.static_this.Log(txt);
		}

		public void LogLn(string line)
		{
			this.logTextBox.Invoke(new MethodInvoker(() => this.logTextBox.AppendText(string.Concat(line, Environment.NewLine))));
		}

		public static void LogLn_(string txt)
		{
			MainWin.static_this.LogLn(txt);
		}

		private void MainWin_Load(object sender, EventArgs e)
		{
			this.LogLn(string.Concat("MoggTools v", Assembly.GetEntryAssembly().GetName().Version.ToString()));
		}

		private void openPackageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				Title = "Select package to open."
			};
			OpenFileDialog openFileDialog1 = openFileDialog;
			openFileDialog1.Filter = string.Concat(openFileDialog1.Filter, "All Files (*.*)|*.*");
			OpenFileDialog openFileDialog2 = openFileDialog;
			openFileDialog2.Filter = string.Concat(openFileDialog2.Filter, "|STFS Package (*.*)|*.*");
			OpenFileDialog openFileDialog3 = openFileDialog;
			openFileDialog3.Filter = string.Concat(openFileDialog3.Filter, "|Ark Package (*.hdr)|*.hdr");
			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.LoadFile(openFileDialog.FileName);
			}
		}

		private void PopulateListView(List<Song> songs)
		{
			this.LogLn(string.Concat("Found ", songs.Count, " song(s)"));
			this.songCountLabel.Text = songs.Count.ToString();
			this.songListView.BeginUpdate();
			foreach (Song song in songs)
			{
				ListViewItem listViewItem = new ListViewItem(new string[] { song.Name, song.Artist, song.Album, song.Year, song.Duration })
				{
					Tag = song
				};
				this.songListView.Items.Add(listViewItem);
			}
			this.statusLabel.Text = "Ready";
			this.songListView.EndUpdate();
			this.EnableAll();
			this.checkAllBtn_Click(null, null);
		}

		private void renameToArtistSongName_Click(object sender, EventArgs e)
		{
			Settings.Default.RenameToArtistSongName = this.renameToArtistSongName.Checked;
			Settings.Default.Save();
		}

		private void saveMIDIDataInRPP_Click(object sender, EventArgs e)
		{
			Settings.Default.SaveMidiDataInRPP = this.saveMIDIDataInRPP.Checked;
			Settings.Default.Save();
		}

		private void saveTempoMapInRPP_Click(object sender, EventArgs e)
		{
			Settings.Default.SaveTempoMapInRPP = this.saveTempoMapInRPP.Checked;
			Settings.Default.Save();
		}

		private void showTutorialSongs_Click(object sender, EventArgs e)
		{
			Settings.Default.ShowTutorialSongs = this.showTutorialSongs.Checked;
			Settings.Default.Save();
		}

		private void songListView_Clicked(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Right && this.songListView.FocusedItem.Bounds.Contains(e.Location))
			{
				this.songListContextMenu.Show(System.Windows.Forms.Cursor.Position);
			}
		}

		private void songListView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (e.Column != this.sorter.SortColumn)
			{
				this.sorter.SortColumn = e.Column;
				this.sorter.Order = SortOrder.Ascending;
			}
			else if (this.sorter.Order != SortOrder.Ascending)
			{
				this.sorter.Order = SortOrder.Ascending;
			}
			else
			{
				this.sorter.Order = SortOrder.Descending;
			}
			this.songListView.Sort();
		}

		private void songListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			if (!e.IsSelected)
			{
				return;
			}
			Song tag = (Song)e.Item.Tag;
		}

		private void uncheckAllBtn_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < this.songListView.Items.Count; i++)
			{
				this.songListView.Items[i].Checked = false;
			}
		}

		private void unloadCurrentPackage_Click(object sender, EventArgs e)
		{
			this.unloadPackage();
		}

		private void unloadPackage()
		{
			if (this.pkg != null)
			{
				this.pkg.Dispose();
			}
			this.pkg = null;
			this.songListView.Items.Clear();
			this.songListView.Enabled = false;
			this.createRppCheckedButton.Enabled = false;
			this.extractCheckedMoggButton.Enabled = false;
			this.decryptCheckedMoggButton.Enabled = false;
			this.extractCheckedMidisBtn.Enabled = false;
			this.extractCheckedDtasBtn.Enabled = false;
			this.unloadCurrentPackageToolStripMenuItem.Enabled = false;
			this.checkAllBtn.Enabled = false;
			this.uncheckAllBtn.Enabled = false;
			this.invertCheckedBtn.Enabled = false;
			this.pictureBox1.Image = null;
			this.pictureBox2.Image = null;
			this.songCountLabel.Text = "";
			this.Text = MainWin.AppName;
		}
	}
}