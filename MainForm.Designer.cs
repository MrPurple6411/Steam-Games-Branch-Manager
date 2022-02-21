namespace Steam_Games_Branch_Manager
{
    partial class MainForm
    {
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
            this.Games = new System.Windows.Forms.ListBox();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.NameOverrideLabel = new System.Windows.Forms.Label();
            this.Branches = new System.Windows.Forms.CheckedListBox();
            this.CreateBranch = new System.Windows.Forms.Button();
            this.DeleteBranch = new System.Windows.Forms.Button();
            this.RenameBranch = new System.Windows.Forms.Button();
            this.AddGameButton = new System.Windows.Forms.Button();
            this.RenameGame = new System.Windows.Forms.Button();
            this.DeleteGame = new System.Windows.Forms.Button();
            this.TryCreateBranchWorker = new System.ComponentModel.BackgroundWorker();
            this.FilesCopiedLabel = new System.Windows.Forms.Label();
            this.DeleteBranchWorker = new System.ComponentModel.BackgroundWorker();
            this.DeleteGameWorker = new System.ComponentModel.BackgroundWorker();
            this.BranchesLabel = new System.Windows.Forms.Label();
            this.GamesLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Games
            // 
            this.Games.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.Games.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Games.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.Games.FormattingEnabled = true;
            this.Games.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Games.Location = new System.Drawing.Point(209, 25);
            this.Games.Name = "Games";
            this.Games.Size = new System.Drawing.Size(218, 290);
            this.Games.TabIndex = 0;
            this.Games.SelectedValueChanged += new System.EventHandler(this.GamesBox_SelectedValueChanged);
            // 
            // NameTextBox
            // 
            this.NameTextBox.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.NameTextBox.ForeColor = System.Drawing.SystemColors.Highlight;
            this.NameTextBox.Location = new System.Drawing.Point(63, 361);
            this.NameTextBox.MaxLength = 12;
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(131, 20);
            this.NameTextBox.TabIndex = 5;
            this.NameTextBox.WordWrap = false;
            // 
            // NameOverrideLabel
            // 
            this.NameOverrideLabel.AutoSize = true;
            this.NameOverrideLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.NameOverrideLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.NameOverrideLabel.Location = new System.Drawing.Point(16, 364);
            this.NameOverrideLabel.Name = "NameOverrideLabel";
            this.NameOverrideLabel.Size = new System.Drawing.Size(47, 13);
            this.NameOverrideLabel.TabIndex = 6;
            this.NameOverrideLabel.Text = "Name: ";
            // 
            // Branches
            // 
            this.Branches.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.Branches.CheckOnClick = true;
            this.Branches.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Branches.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.Branches.FormattingEnabled = true;
            this.Branches.Location = new System.Drawing.Point(16, 26);
            this.Branches.Name = "Branches";
            this.Branches.Size = new System.Drawing.Size(178, 289);
            this.Branches.TabIndex = 10;
            this.Branches.ThreeDCheckBoxes = true;
            this.Branches.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.Branches_ItemCheck);
            this.Branches.SelectedValueChanged += new System.EventHandler(this.Branches_SelectedValueChanged);
            // 
            // CreateBranch
            // 
            this.CreateBranch.Location = new System.Drawing.Point(200, 359);
            this.CreateBranch.Name = "CreateBranch";
            this.CreateBranch.Size = new System.Drawing.Size(100, 23);
            this.CreateBranch.TabIndex = 11;
            this.CreateBranch.Text = "Create Branch";
            this.CreateBranch.UseVisualStyleBackColor = true;
            this.CreateBranch.Click += new System.EventHandler(this.CreateBranch_Click);
            // 
            // DeleteBranch
            // 
            this.DeleteBranch.Location = new System.Drawing.Point(107, 323);
            this.DeleteBranch.Name = "DeleteBranch";
            this.DeleteBranch.Size = new System.Drawing.Size(87, 23);
            this.DeleteBranch.TabIndex = 12;
            this.DeleteBranch.Text = "Delete";
            this.DeleteBranch.UseVisualStyleBackColor = true;
            this.DeleteBranch.Click += new System.EventHandler(this.DeleteBranch_Click);
            // 
            // RenameBranch
            // 
            this.RenameBranch.Location = new System.Drawing.Point(16, 323);
            this.RenameBranch.Name = "RenameBranch";
            this.RenameBranch.Size = new System.Drawing.Size(87, 23);
            this.RenameBranch.TabIndex = 13;
            this.RenameBranch.Text = "Rename";
            this.RenameBranch.UseVisualStyleBackColor = true;
            this.RenameBranch.Click += new System.EventHandler(this.RenameBranch_Click);
            // 
            // AddGameButton
            // 
            this.AddGameButton.Location = new System.Drawing.Point(306, 359);
            this.AddGameButton.Name = "AddGameButton";
            this.AddGameButton.Size = new System.Drawing.Size(121, 23);
            this.AddGameButton.TabIndex = 14;
            this.AddGameButton.Text = "Add Game";
            this.AddGameButton.UseVisualStyleBackColor = true;
            this.AddGameButton.Click += new System.EventHandler(this.AddGameButton_Click);
            // 
            // RenameGame
            // 
            this.RenameGame.Location = new System.Drawing.Point(209, 321);
            this.RenameGame.Name = "RenameGame";
            this.RenameGame.Size = new System.Drawing.Size(99, 23);
            this.RenameGame.TabIndex = 16;
            this.RenameGame.Text = "Rename";
            this.RenameGame.UseVisualStyleBackColor = true;
            this.RenameGame.Click += new System.EventHandler(this.RenameGame_Click);
            // 
            // DeleteGame
            // 
            this.DeleteGame.Location = new System.Drawing.Point(324, 321);
            this.DeleteGame.Name = "DeleteGame";
            this.DeleteGame.Size = new System.Drawing.Size(103, 23);
            this.DeleteGame.TabIndex = 15;
            this.DeleteGame.Text = "Delete";
            this.DeleteGame.UseVisualStyleBackColor = true;
            this.DeleteGame.Click += new System.EventHandler(this.DeleteGame_Click);
            // 
            // TryCreateBranchWorker
            // 
            this.TryCreateBranchWorker.WorkerReportsProgress = true;
            this.TryCreateBranchWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.TryCreateBranchWorker_DoWork);
            this.TryCreateBranchWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.TryCreateBranchWorker_ProgressChanged);
            this.TryCreateBranchWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.TryCreateBranchWorker_RunWorkerCompleted);
            // 
            // FilesCopiedLabel
            // 
            this.FilesCopiedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FilesCopiedLabel.ForeColor = System.Drawing.SystemColors.Info;
            this.FilesCopiedLabel.Location = new System.Drawing.Point(24, 390);
            this.FilesCopiedLabel.Name = "FilesCopiedLabel";
            this.FilesCopiedLabel.Size = new System.Drawing.Size(403, 23);
            this.FilesCopiedLabel.TabIndex = 17;
            // 
            // DeleteBranchWorker
            // 
            this.DeleteBranchWorker.WorkerReportsProgress = true;
            this.DeleteBranchWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DeleteBranchWorker_DoWork);
            this.DeleteBranchWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.DeleteBranchWorker_ProgressChanged);
            this.DeleteBranchWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.DeleteBranchWorker_RunWorkerCompleted);
            // 
            // DeleteGameWorker
            // 
            this.DeleteGameWorker.WorkerReportsProgress = true;
            this.DeleteGameWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DeleteGameWorker_DoWork);
            this.DeleteGameWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.DeleteGameWorker_ProgressChanged);
            this.DeleteGameWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.DeleteGameWorker_RunWorkerCompleted);
            // 
            // BranchesLabel
            // 
            this.BranchesLabel.Location = new System.Drawing.Point(16, 11);
            this.BranchesLabel.Name = "BranchesLabel";
            this.BranchesLabel.Size = new System.Drawing.Size(178, 12);
            this.BranchesLabel.TabIndex = 18;
            this.BranchesLabel.Text = "Branches";
            // 
            // GamesLabel
            // 
            this.GamesLabel.Location = new System.Drawing.Point(209, 9);
            this.GamesLabel.Name = "GamesLabel";
            this.GamesLabel.Size = new System.Drawing.Size(218, 12);
            this.GamesLabel.TabIndex = 19;
            this.GamesLabel.Text = "Games";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(444, 422);
            this.Controls.Add(this.GamesLabel);
            this.Controls.Add(this.BranchesLabel);
            this.Controls.Add(this.FilesCopiedLabel);
            this.Controls.Add(this.RenameGame);
            this.Controls.Add(this.DeleteGame);
            this.Controls.Add(this.AddGameButton);
            this.Controls.Add(this.RenameBranch);
            this.Controls.Add(this.DeleteBranch);
            this.Controls.Add(this.CreateBranch);
            this.Controls.Add(this.Branches);
            this.Controls.Add(this.NameOverrideLabel);
            this.Controls.Add(this.NameTextBox);
            this.Controls.Add(this.Games);
            this.Name = "MainForm";
            this.Text = "Steam Games Branch Manager";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label BranchesLabel;
        private System.Windows.Forms.Label GamesLabel;

        private System.ComponentModel.BackgroundWorker DeleteBranchWorker;
        private System.ComponentModel.BackgroundWorker DeleteGameWorker;

        private System.Windows.Forms.Label FilesCopiedLabel;

        private System.ComponentModel.BackgroundWorker TryCreateBranchWorker;

        private System.Windows.Forms.Button RenameGame;
        private System.Windows.Forms.Button DeleteGame;

        private System.Windows.Forms.Button AddGameButton;

        #endregion

        private System.Windows.Forms.ListBox Games;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Label NameOverrideLabel;
        private System.Windows.Forms.CheckedListBox Branches;
        private System.Windows.Forms.Button CreateBranch;
        private System.Windows.Forms.Button DeleteBranch;
        private System.Windows.Forms.Button RenameBranch;
    }
}

