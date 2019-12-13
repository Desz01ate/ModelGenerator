using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModelGenerator.Components
{
    public partial class DisposableMessageBox : Form
    {
        public DisposableMessageBox()
        {
            InitializeComponent();
            SequencedInitialize();

        }
        public DisposableMessageBox(string message)
        {
            InitializeComponent();
            SequencedInitialize();
            this.Message.Text = message;
        }
        public void SequencedInitialize()
        {
            FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
            var stw = new Stopwatch();
            stw.Start();
            var timer = new Timer();
            timer.Tick += (s, e) =>
            {
                this.Elapsed.Text = $"Elapsed for {(int)(stw.ElapsedMilliseconds / 1000)} seconds";
            };
            timer.Interval = 1000;
            timer.Start();
        }

        private void DisposableMessageBox_Load(object sender, EventArgs e)
        {

        }
    }
}
