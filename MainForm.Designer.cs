namespace Steam_Games_Branch_Manager
{
    partial class MainForm
    {
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.TableLayoutPanel buttonsLayout;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.buttonsLayout = new System.Windows.Forms.TableLayoutPanel();
            this.Games = new System.Windows.Forms.ListBox();
            this.Branches = new System.Windows.Forms.CheckedListBox();
            this.GamesLabel = new System.Windows.Forms.Label();
            this.BranchesLabel = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.NameOverrideLabel = new System.Windows.Forms.Label();
            this.CreateBranch = new System.Windows.Forms.Button();
            this.DeleteBranch = new System.Windows.Forms.Button();
            this.RenameBranch = new System.Windows.Forms.Button();
            this.AddGameButton = new System.Windows.Forms.Button();
            this.RenameGame = new System.Windows.Forms.Button();
            this.DeleteGame = new System.Windows.Forms.Button();
            this.FilesCopiedLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            // Initialize BackgroundWorkers
            this.TryCreateBranchWorker = new System.ComponentModel.BackgroundWorker();
            this.DeleteGameWorker = new System.ComponentModel.BackgroundWorker();
            this.DeleteBranchWorker = new System.ComponentModel.BackgroundWorker();

            // Configure TryCreateBranchWorker
            this.TryCreateBranchWorker.WorkerReportsProgress = true;
            this.TryCreateBranchWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.TryCreateBranchWorker_DoWork);
            this.TryCreateBranchWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.TryCreateBranchWorker_ProgressChanged);
            this.TryCreateBranchWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.TryCreateBranchWorker_RunWorkerCompleted);

            // Configure DeleteGameWorker
            this.DeleteGameWorker.WorkerReportsProgress = true;
            this.DeleteGameWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DeleteGameWorker_DoWork);
            this.DeleteGameWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.DeleteGameWorker_ProgressChanged);
            this.DeleteGameWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.DeleteGameWorker_RunWorkerCompleted);

            // Configure DeleteBranchWorker
            this.DeleteBranchWorker.WorkerReportsProgress = true;
            this.DeleteBranchWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DeleteBranchWorker_DoWork);
            this.DeleteBranchWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.DeleteBranchWorker_ProgressChanged);
            this.DeleteBranchWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.DeleteBranchWorker_RunWorkerCompleted);

            this.menuStrip1.SuspendLayout();
            this.mainLayout.SuspendLayout();
            this.buttonsLayout.SuspendLayout();
            this.SuspendLayout();

            // menuStrip1
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.helpToolStripMenuItem
            });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(600, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";

            // helpToolStripMenuItem
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.aboutToolStripMenuItem
            });
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";

            // aboutToolStripMenuItem
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);

            // mainLayout
            this.mainLayout.ColumnCount = 2;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainLayout.Controls.Add(this.BranchesLabel, 0, 0);
            this.mainLayout.Controls.Add(this.GamesLabel, 1, 0);
            this.mainLayout.Controls.Add(this.Branches, 0, 1);
            this.mainLayout.Controls.Add(this.Games, 1, 1);
            this.mainLayout.Controls.Add(this.buttonsLayout, 0, 2);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 24);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 3;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.mainLayout.Size = new System.Drawing.Size(600, 400);
            this.mainLayout.TabIndex = 1;

            // buttonsLayout
            this.buttonsLayout.ColumnCount = 6;
            this.mainLayout.SetColumnSpan(this.buttonsLayout, 2);
            this.buttonsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.buttonsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.buttonsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.buttonsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.buttonsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.buttonsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.buttonsLayout.Controls.Add(this.NameOverrideLabel, 0, 0);
            this.buttonsLayout.Controls.Add(this.NameTextBox, 1, 0);
            this.buttonsLayout.Controls.Add(this.CreateBranch, 2, 0);
            this.buttonsLayout.Controls.Add(this.DeleteBranch, 3, 0);
            this.buttonsLayout.Controls.Add(this.RenameBranch, 4, 0);
            this.buttonsLayout.Controls.Add(this.AddGameButton, 5, 0);
            this.buttonsLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonsLayout.Location = new System.Drawing.Point(3, 323);
            this.buttonsLayout.Name = "buttonsLayout";
            this.buttonsLayout.RowCount = 1;
            this.buttonsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.buttonsLayout.Size = new System.Drawing.Size(594, 74);
            this.buttonsLayout.TabIndex = 0;

            // BranchesLabel
            this.BranchesLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BranchesLabel.Text = "Branches";
            this.BranchesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // GamesLabel
            this.GamesLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GamesLabel.Text = "Games";
            this.GamesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Branches
            this.Branches.Dock = System.Windows.Forms.DockStyle.Fill;

            // Games
            this.Games.Dock = System.Windows.Forms.DockStyle.Fill;

            // NameOverrideLabel
            this.NameOverrideLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameOverrideLabel.Text = "Name:";
            this.NameOverrideLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // NameTextBox
            this.NameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.NameTextBox.Margin = new System.Windows.Forms.Padding(5, 10, 5, 10);

            // CreateBranch
            this.CreateBranch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CreateBranch.Text = "Create Branch";
            this.CreateBranch.Click += new System.EventHandler(this.CreateBranch_Click);

            // DeleteBranch
            this.DeleteBranch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeleteBranch.Text = "Delete Branch";
            this.DeleteBranch.Click += new System.EventHandler(this.DeleteBranch_Click);

            // RenameBranch
            this.RenameBranch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RenameBranch.Text = "Rename Branch";
            this.RenameBranch.Click += new System.EventHandler(this.RenameBranch_Click);

            // AddGameButton
            this.AddGameButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddGameButton.Text = "Add Game";
            this.AddGameButton.Click += new System.EventHandler(this.AddGameButton_Click);

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Steam Games Branch Manager";

            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mainLayout.ResumeLayout(false);
            this.mainLayout.PerformLayout();
            this.buttonsLayout.ResumeLayout(false);
            this.buttonsLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox Games;
        private System.Windows.Forms.CheckedListBox Branches;
        private System.Windows.Forms.Label GamesLabel;
        private System.Windows.Forms.Label BranchesLabel;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Label NameOverrideLabel;
        private System.Windows.Forms.Button CreateBranch;
        private System.Windows.Forms.Button DeleteBranch;
        private System.Windows.Forms.Button RenameBranch;
        private System.Windows.Forms.Button AddGameButton;
        private System.Windows.Forms.Button RenameGame;
        private System.Windows.Forms.Button DeleteGame;
        private System.Windows.Forms.Label FilesCopiedLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;

        // Declare BackgroundWorkers
        private System.ComponentModel.BackgroundWorker TryCreateBranchWorker;
        private System.ComponentModel.BackgroundWorker DeleteGameWorker;
        private System.ComponentModel.BackgroundWorker DeleteBranchWorker;
    }
}

