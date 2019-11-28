using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ModelGenerator.Components
{
    public class DisposableMessageBox : Form
    {
        private Label lbl_Message;
        private Label lbl_Count;
        private Timer _timer;
        private readonly Stopwatch stopwatch = new Stopwatch();
        public DisposableMessageBox(string message)
        {
            InitializeComponent();
            Initializer();
            this.lbl_Message.Text = message;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            stopwatch.Stop();

            base.OnClosing(e);
        }
        private void Initializer()
        {
            this.TopMost = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            stopwatch.Start();
            _timer = new Timer
            {
                Interval = 500
            };
            _timer.Tick += (s, e) =>
            {
                lbl_Count.Text = $"Elapsed time : {stopwatch.ElapsedMilliseconds / 1000} seconds...";
            };
            _timer.Start();
        }

        private void InitializeComponent()
        {
            this.lbl_Message = new System.Windows.Forms.Label();
            this.lbl_Count = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_Message
            // 
            this.lbl_Message.AutoSize = true;
            this.lbl_Message.Location = new System.Drawing.Point(25, 109);
            this.lbl_Message.Name = "lbl_Message";
            this.lbl_Message.Size = new System.Drawing.Size(14, 13);
            this.lbl_Message.TabIndex = 0;
            this.lbl_Message.Text = "X";
            // 
            // lbl_Count
            // 
            this.lbl_Count.AutoSize = true;
            this.lbl_Count.Location = new System.Drawing.Point(25, 239);
            this.lbl_Count.Name = "lbl_Count";
            this.lbl_Count.Size = new System.Drawing.Size(35, 13);
            this.lbl_Count.TabIndex = 1;
            this.lbl_Count.Text = "label1";
            // 
            // DisposableMessageBox
            // 
            this.ClientSize = new System.Drawing.Size(504, 261);
            this.Controls.Add(this.lbl_Count);
            this.Controls.Add(this.lbl_Message);
            this.Name = "DisposableMessageBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
