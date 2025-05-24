namespace Steam_Games_Branch_Manager
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage GamesTabPage;
        private System.Windows.Forms.TabPage BranchesTabPage;
        // Games tab controls
        private System.Windows.Forms.ListBox Games;
        private System.Windows.Forms.Button AddGameButton;
        private System.Windows.Forms.Button DeleteGame;
        private System.Windows.Forms.Label GamesLabel;
        // Branches tab controls
        private System.Windows.Forms.ListBox BranchesListBox;
        private System.Windows.Forms.Button AddBranchButton;
        private System.Windows.Forms.Button RenameBranchButton;
        private System.Windows.Forms.Button DeleteBranchButton;
        private System.Windows.Forms.TextBox BranchNameTextBox;
        private System.Windows.Forms.Label BranchesLabel;
        private System.Windows.Forms.Label BranchNameLabel;
        private System.Windows.Forms.Label BranchesTabGameLabel;

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
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.GamesTabPage = new System.Windows.Forms.TabPage();
            this.BranchesTabPage = new System.Windows.Forms.TabPage();
            // Games tab controls
            this.Games = new System.Windows.Forms.ListBox();
            this.AddGameButton = new System.Windows.Forms.Button();
            this.DeleteGame = new System.Windows.Forms.Button();
            this.GamesLabel = new System.Windows.Forms.Label();
            // Branches tab controls
            this.BranchesListBox = new System.Windows.Forms.ListBox();
            this.AddBranchButton = new System.Windows.Forms.Button();
            this.RenameBranchButton = new System.Windows.Forms.Button();
            this.DeleteBranchButton = new System.Windows.Forms.Button();
            this.BranchNameTextBox = new System.Windows.Forms.TextBox();
            this.BranchesLabel = new System.Windows.Forms.Label();
            this.BranchNameLabel = new System.Windows.Forms.Label();
            this.BranchesTabGameLabel = new System.Windows.Forms.Label();
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.GamesTabPage);
            this.MainTabControl.Controls.Add(this.BranchesTabPage);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(400, 400);
            this.MainTabControl.TabIndex = 0;
            // 
            // GamesTabPage
            // 
            this.GamesTabPage.Controls.Add(this.GamesLabel);
            this.GamesTabPage.Controls.Add(this.Games);
            this.GamesTabPage.Controls.Add(this.AddGameButton);
            this.GamesTabPage.Controls.Add(this.DeleteGame);
            this.GamesTabPage.Location = new System.Drawing.Point(4, 22);
            this.GamesTabPage.Name = "GamesTabPage";
            this.GamesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.GamesTabPage.Size = new System.Drawing.Size(392, 374);
            this.GamesTabPage.TabIndex = 0;
            this.GamesTabPage.Text = "Games";
            this.GamesTabPage.UseVisualStyleBackColor = true;
            // 
            // BranchesTabPage
            // 
            this.BranchesTabPage.Controls.Add(this.BranchesLabel);
            this.BranchesTabPage.Controls.Add(this.BranchesListBox);
            this.BranchesTabPage.Controls.Add(this.BranchNameLabel);
            this.BranchesTabPage.Controls.Add(this.BranchNameTextBox);
            this.BranchesTabPage.Controls.Add(this.AddBranchButton);
            this.BranchesTabPage.Controls.Add(this.RenameBranchButton);
            this.BranchesTabPage.Controls.Add(this.DeleteBranchButton);
            this.BranchesTabPage.Controls.Add(this.BranchesTabGameLabel);
            this.BranchesTabPage.Location = new System.Drawing.Point(4, 22);
            this.BranchesTabPage.Name = "BranchesTabPage";
            this.BranchesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.BranchesTabPage.Size = new System.Drawing.Size(392, 374);
            this.BranchesTabPage.TabIndex = 1;
            this.BranchesTabPage.Text = "Branches";
            this.BranchesTabPage.UseVisualStyleBackColor = true;
            // 
            // Games controls layout
            // 
            this.GamesLabel.AutoSize = true;
            this.GamesLabel.Location = new System.Drawing.Point(10, 10);
            this.GamesLabel.Name = "GamesLabel";
            this.GamesLabel.Size = new System.Drawing.Size(42, 13);
            this.GamesLabel.Text = "Games";
            this.Games.Location = new System.Drawing.Point(10, 30);
            this.Games.Size = new System.Drawing.Size(250, 200);
            this.Games.SelectedValueChanged += new System.EventHandler(this.GamesBox_SelectedValueChanged);
            this.AddGameButton.Location = new System.Drawing.Point(10, 270);
            this.AddGameButton.Size = new System.Drawing.Size(80, 30);
            this.AddGameButton.Text = "Add Game";
            this.AddGameButton.Click += new System.EventHandler(this.AddGameButton_Click);
            this.DeleteGame.Location = new System.Drawing.Point(190, 270);
            this.DeleteGame.Size = new System.Drawing.Size(80, 30);
            this.DeleteGame.Text = "Unmanage";
            this.DeleteGame.Click += new System.EventHandler(this.DeleteGame_Click);
            // 
            // Branches controls layout
            // 
            this.BranchesLabel.AutoSize = true;
            this.BranchesLabel.Location = new System.Drawing.Point(10, 10);
            this.BranchesLabel.Text = "Branches";
            this.BranchesListBox.Location = new System.Drawing.Point(10, 30);
            this.BranchesListBox.Size = new System.Drawing.Size(250, 200);
            this.BranchNameLabel.AutoSize = true;
            this.BranchNameLabel.Location = new System.Drawing.Point(10, 240);
            this.BranchNameLabel.Text = "Name:";
            this.BranchNameTextBox.Location = new System.Drawing.Point(60, 237);
            this.BranchNameTextBox.Size = new System.Drawing.Size(200, 20);
            this.AddBranchButton.Location = new System.Drawing.Point(10, 270);
            this.AddBranchButton.Size = new System.Drawing.Size(80, 30);
            this.AddBranchButton.Text = "Add";
            this.RenameBranchButton.Location = new System.Drawing.Point(100, 270);
            this.RenameBranchButton.Size = new System.Drawing.Size(80, 30);
            this.RenameBranchButton.Text = "Rename";
            this.DeleteBranchButton.Location = new System.Drawing.Point(190, 270);
            this.DeleteBranchButton.Size = new System.Drawing.Size(80, 30);
            this.DeleteBranchButton.Text = "Delete";
            this.BranchesTabGameLabel.AutoSize = true;
            this.BranchesTabGameLabel.Location = new System.Drawing.Point(120, 10);
            this.BranchesTabGameLabel.Name = "BranchesTabGameLabel";
            this.BranchesTabGameLabel.Size = new System.Drawing.Size(0, 13);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.MainTabControl);
            this.Name = "MainForm";
            this.Text = "Steam Games Branch Manager";
            this.ResumeLayout(false);
        }

        #endregion
    }
}

