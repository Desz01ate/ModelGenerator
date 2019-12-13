namespace ModelGenerator.Components
{
    partial class DisposableMessageBox
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
            this.Message = new System.Windows.Forms.Label();
            this.Elapsed = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.Message.Location = new System.Drawing.Point(12, 9);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(35, 13);
            this.Message.TabIndex = 0;
            this.Message.Text = "label1";
            // 
            // Elapsed
            // 
            this.Elapsed.AutoSize = true;
            this.Elapsed.Location = new System.Drawing.Point(12, 75);
            this.Elapsed.Name = "Elapsed";
            this.Elapsed.Size = new System.Drawing.Size(35, 13);
            this.Elapsed.TabIndex = 1;
            this.Elapsed.Text = "label1";
            // 
            // DisposableMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 97);
            this.Controls.Add(this.Elapsed);
            this.Controls.Add(this.Message);
            this.Name = "DisposableMessageBox";
            this.Text = "DisposableMessageBox";
            this.Load += new System.EventHandler(this.DisposableMessageBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Message;
        private System.Windows.Forms.Label Elapsed;
    }
}