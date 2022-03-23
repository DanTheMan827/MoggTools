using DtxCS;
using GameArchives;
using GameArchives.PFS;
using GameArchives.STFS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoggTools
{
    internal class BatchWindow : Form
    {
        private bool completed;

        private CancellationTokenSource cts;

        private Progress<int> progress;

        private Progress<string> logger;

        private IContainer components;

        private ProgressBar progressBar;

        private TextBox logBox;

        private Label logLabel;

        private Button cancelButton;

        public BatchWindow()
        {
            this.InitializeComponent();
            this.progress = new Progress<int>((int percent) =>
            {
                this.progressBar.Value = percent;
                if (percent == 100)
                {
                    this.completed = true;
                    this.cancelButton.Text = "Close";
                }
            });
            this.logger = new Progress<string>((string message) => this.LogLn(message));
            this.cts = new CancellationTokenSource();
        }

        public async void batchExtractMids(string indir, string outdir)
        {
            Action<AbstractPackage, Song, IProgress<string>> action2 = null;
            await Task.Run(() =>
            {
                BatchWindow u003cu003e4_this = this;
                string str = indir;
                Progress<int> progress = this.progress;
                Progress<string> u003cu003e4_this1 = this.logger;
                Action<AbstractPackage, Song, IProgress<string>> u003cu003e9_1 = action2;
                if (u003cu003e9_1 == null)
                {
                    Action<AbstractPackage, Song, IProgress<string>> action = (AbstractPackage pkg, Song song, IProgress<string> log) => Util.ExtractTo(pkg.GetFile(string.Concat(song.Path, ".mid")), Path.Combine(outdir, string.Concat(song.GetFileName(), ".mid")));
                    Action<AbstractPackage, Song, IProgress<string>> action1 = action;
                    action2 = action;
                    u003cu003e9_1 = action1;
                }
                u003cu003e4_this.DoForAllPkgs(str, ".mid", progress, u003cu003e4_this1, u003cu003e9_1, this.cts.Token);
            });
        }

        public async void batchExtractMoggs(string indir, string outdir)
        {
            Action<AbstractPackage, Song, IProgress<string>> action2 = null;
            await Task.Run(() =>
            {
                BatchWindow u003cu003e4_this = this;
                string str = indir;
                Progress<int> u003cu003e4_this1 = this.progress;
                Progress<string> progress1 = this.logger;
                Action<AbstractPackage, Song, IProgress<string>> u003cu003e9_1 = action2;
                if (u003cu003e9_1 == null)
                {
                    Action<AbstractPackage, Song, IProgress<string>> action = (AbstractPackage pkg, Song song, IProgress<string> log) =>
                    {
                        IProgress<string> progress = log;
                        Utils.SaveMoggToFile(song.ShortName, Path.Combine(outdir, string.Concat(song.GetFileName(), ".mogg")), pkg.GetFile(string.Concat(song.Path, ".mogg")).GetBytes(), new Action<string>(progress.Report), false);
                    };
                    Action<AbstractPackage, Song, IProgress<string>> action1 = action;
                    action2 = action;
                    u003cu003e9_1 = action1;
                }
                u003cu003e4_this.DoForAllPkgs(str, ".mogg", u003cu003e4_this1, progress1, u003cu003e9_1, this.cts.Token);
            });
        }

        public async void batchExtractMoggsAndRPPs(string indir, string outdir)
        {
            Action<AbstractPackage, Song, IProgress<string>> action2 = null;
            await Task.Run(() =>
            {
                BatchWindow u003cu003e4_this = this;
                string str = indir;
                Progress<int> u003cu003e4_this1 = this.progress;
                Progress<string> progress1 = this.logger;
                Action<AbstractPackage, Song, IProgress<string>> u003cu003e9_1 = action2;
                if (u003cu003e9_1 == null)
                {
                    Action<AbstractPackage, Song, IProgress<string>> action = (AbstractPackage pkg, Song song, IProgress<string> log) =>
                    {
                        IProgress<string> progress = log;
                        Utils.SaveMoggToFile(song.ShortName, Path.Combine(outdir, string.Concat(song.GetFileName(), ".mogg")), pkg.GetFile(string.Concat(song.Path, ".mogg")).GetBytes(), new Action<string>(progress.Report), false);
                        Utils.SaveRpp(song, outdir);
                    };
                    Action<AbstractPackage, Song, IProgress<string>> action1 = action;
                    action2 = action;
                    u003cu003e9_1 = action1;
                }
                u003cu003e4_this.DoForAllPkgs(str, ".rpp/.mogg", u003cu003e4_this1, progress1, u003cu003e9_1, this.cts.Token);
            });
        }

        public async void batchExtractRPPs(string indir, string outdir)
        {
            Action<AbstractPackage, Song, IProgress<string>> action2 = null;
            await Task.Run(() =>
            {
                BatchWindow u003cu003e4_this = this;
                string str = indir;
                Progress<int> progress = this.progress;
                Progress<string> u003cu003e4_this1 = this.logger;
                Action<AbstractPackage, Song, IProgress<string>> u003cu003e9_1 = action2;
                if (u003cu003e9_1 == null)
                {
                    Action<AbstractPackage, Song, IProgress<string>> action = (AbstractPackage pkg, Song song, IProgress<string> log) => Utils.SaveRpp(song, outdir);
                    Action<AbstractPackage, Song, IProgress<string>> action1 = action;
                    action2 = action;
                    u003cu003e9_1 = action1;
                }
                u003cu003e4_this.DoForAllPkgs(str, ".rpp", progress, u003cu003e4_this1, u003cu003e9_1, this.cts.Token);
            });
        }

        public async void batchSaveDTAs(string indir, string outdir)
        {
            Action<AbstractPackage, Song, IProgress<string>> action2 = null;
            await Task.Run(() =>
            {
                BatchWindow u003cu003e4_this = this;
                string str = indir;
                Progress<int> progress = this.progress;
                Progress<string> u003cu003e4_this1 = this.logger;
                Action<AbstractPackage, Song, IProgress<string>> u003cu003e9_1 = action2;
                if (u003cu003e9_1 == null)
                {
                    Action<AbstractPackage, Song, IProgress<string>> action = (AbstractPackage pkg, Song song, IProgress<string> log) => Utils.SaveDTA(song, outdir);
                    Action<AbstractPackage, Song, IProgress<string>> action1 = action;
                    action2 = action;
                    u003cu003e9_1 = action1;
                }
                u003cu003e4_this.DoForAllPkgs(str, ".dta", progress, u003cu003e4_this1, u003cu003e9_1, this.cts.Token);
            });
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (this.completed)
            {
                base.Close();
                base.Dispose();
                return;
            }
            if (MessageBox.Show("Are you sure you want to cancel the batch operation?", "Confirm Cancel", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                this.cts.Cancel();
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

        private void DoForAllPkgs(string indir, string ext, IProgress<int> progress, IProgress<string> logger, Action<AbstractPackage, Song, IProgress<string>> action, CancellationToken ct)
        {
            AbstractPackage abstractPackage;
            IProgress<string> progress1;
            string str;
            progress.Report(0);
            List<IFile> files = new List<IFile>();
            List<IFile> files1 = new List<IFile>();
            foreach (IFile file in Util.LocalDir(indir).Files)
            {
                if (STFSPackage.IsSTFS(file) == PackageTestResult.YES)
                {
                    files.Add(file);
                }
                if (PFSPackage.IsPFS(file) != PackageTestResult.YES)
                {
                    continue;
                }
                files1.Add(file);
            }
            int count = files.Count + files1.Count;
            int num = 0;
            logger.Report(string.Format("Found {0} supported packages ({1} STFS and {2} PFS)", count, files.Count, files1.Count));
            foreach (IFile file1 in files)
            {
                if (!ct.IsCancellationRequested)
                {
                    try
                    {
                        STFSPackage sTFSPackage = STFSPackage.OpenFile(file1);
                        abstractPackage = sTFSPackage;
                        using (sTFSPackage)
                        {
                            foreach (Song song in Utils.LoadSongs(DTX.FromPlainTextBytes(abstractPackage.GetFile("songs/songs.dta").GetBytes()), abstractPackage))
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    break;
                                }
                                action(abstractPackage, song, logger);
                                logger.Report(string.Concat("Saved ", song.GetFileName(), ext));
                            }
                        }
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        logger.Report(string.Concat("Couldn't process ", file1.Name, ":"));
                        logger.Report(exception.Message);
                    }
                    num++;
                    progress.Report(num * 100 / count);
                }
                else
                {
                    progress.Report(100);
                    progress1 = logger;
                    str = (ct.IsCancellationRequested ? "Cancelled!" : "Done.");
                    progress1.Report(str);
                    return;
                }
            }
            foreach (IFile file2 in files1)
            {
                if (!ct.IsCancellationRequested)
                {
                    try
                    {
                        PFSPackage pFSPackage = PFSPackage.OpenFile(file2, (string x) => x);
                        abstractPackage = pFSPackage;
                        using (pFSPackage)
                        {
                            foreach (Song song1 in Utils.LoadSongsForge(abstractPackage.RootDirectory, "ps4"))
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    break;
                                }
                                action(abstractPackage, song1, logger);
                                logger.Report(string.Concat("Saved ", song1.GetFileName(), ext));
                            }
                        }
                    }
                    catch (Exception exception3)
                    {
                        Exception exception2 = exception3;
                        logger.Report(string.Concat("Couldn't process ", file2.Name, ":"));
                        logger.Report(exception2.Message);
                    }
                    num++;
                    progress.Report(num * 100 / count);
                }
                else
                {
                    progress.Report(100);
                    progress1 = logger;
                    str = (ct.IsCancellationRequested ? "Cancelled!" : "Done.");
                    progress1.Report(str);
                    return;
                }
            }
            progress.Report(100);
            progress1 = logger;
            str = (ct.IsCancellationRequested ? "Cancelled!" : "Done.");
            progress1.Report(str);
        }

        private void InitializeComponent()
        {
            this.progressBar = new ProgressBar();
            this.logBox = new TextBox();
            this.logLabel = new Label();
            this.cancelButton = new Button();
            base.SuspendLayout();
            this.progressBar.Location = new Point(12, 184);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(470, 36);
            this.progressBar.TabIndex = 0;
            this.logBox.Location = new Point(12, 29);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.Size = new System.Drawing.Size(470, 149);
            this.logBox.TabIndex = 2;
            this.logLabel.AutoSize = true;
            this.logLabel.Location = new Point(13, 13);
            this.logLabel.Name = "logLabel";
            this.logLabel.Size = new System.Drawing.Size(25, 13);
            this.logLabel.TabIndex = 3;
            this.logLabel.Text = "Log";
            this.cancelButton.Dock = DockStyle.Bottom;
            this.cancelButton.Location = new Point(0, 226);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(494, 30);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel Operation";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new EventHandler(this.cancelButton_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new System.Drawing.Size(494, 256);
            base.ControlBox = false;
            base.Controls.Add(this.cancelButton);
            base.Controls.Add(this.logLabel);
            base.Controls.Add(this.logBox);
            base.Controls.Add(this.progressBar);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "BatchWindow";
            base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Batch Action";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LogLn(string message)
        {
            this.logBox.AppendText(string.Concat(message, Environment.NewLine));
        }
    }
}