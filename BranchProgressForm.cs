using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Steam_Games_Branch_Manager
{
    public partial class BranchProgressForm : Form
    {
        public bool CancelRequested { get; private set; } = false;
        public BranchProgressForm()
        {
            InitializeComponent();
            btnOk.Enabled = false;
            btnCancel.Enabled = true;
            progressBar.Style = ProgressBarStyle.Continuous;
            lblStatus.Text = "";
        }
        public void SetProgress(int percent, string fileName)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetProgress(percent, fileName)));
                return;
            }
            progressBar.Value = percent;
            lblStatus.Text = fileName;
        }
        public void SetComplete()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(SetComplete));
                return;
            }
            btnOk.Enabled = true;
            btnCancel.Enabled = false;
            lblStatus.Text = "Complete!";
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelRequested = true;
            btnCancel.Enabled = false;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
