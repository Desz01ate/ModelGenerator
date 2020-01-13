namespace ModelGenerator
{
    partial class GeneratorForm
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
            this.btn_Generate = new System.Windows.Forms.Button();
            this.txt_ConnectionString = new System.Windows.Forms.RichTextBox();
            this.cb_GeneratorMode = new System.Windows.Forms.ComboBox();
            this.cb_TargetLang = new System.Windows.Forms.ComboBox();
            this.cb_TargetDatabase = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_AutoReload = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btn_Generate
            // 
            this.btn_Generate.Location = new System.Drawing.Point(12, 257);
            this.btn_Generate.Name = "btn_Generate";
            this.btn_Generate.Size = new System.Drawing.Size(536, 43);
            this.btn_Generate.TabIndex = 0;
            this.btn_Generate.Text = "Generate";
            this.btn_Generate.UseVisualStyleBackColor = true;
            this.btn_Generate.Click += new System.EventHandler(this.Button1_Click);
            // 
            // txt_ConnectionString
            // 
            this.txt_ConnectionString.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_ConnectionString.Location = new System.Drawing.Point(170, 127);
            this.txt_ConnectionString.Name = "txt_ConnectionString";
            this.txt_ConnectionString.Size = new System.Drawing.Size(370, 72);
            this.txt_ConnectionString.TabIndex = 1;
            this.txt_ConnectionString.Text = "";
            // 
            // cb_GeneratorMode
            // 
            this.cb_GeneratorMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_GeneratorMode.FormattingEnabled = true;
            this.cb_GeneratorMode.Items.AddRange(new object[] {
            "Model",
            "Unit of Work",
            "Controller"});
            this.cb_GeneratorMode.Location = new System.Drawing.Point(170, 12);
            this.cb_GeneratorMode.Name = "cb_GeneratorMode";
            this.cb_GeneratorMode.Size = new System.Drawing.Size(370, 21);
            this.cb_GeneratorMode.TabIndex = 2;
            this.cb_GeneratorMode.SelectedIndexChanged += new System.EventHandler(this.Cb_GeneratorMode_SelectedIndexChanged);
            // 
            // cb_TargetLang
            // 
            this.cb_TargetLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_TargetLang.FormattingEnabled = true;
            this.cb_TargetLang.Location = new System.Drawing.Point(170, 51);
            this.cb_TargetLang.Name = "cb_TargetLang";
            this.cb_TargetLang.Size = new System.Drawing.Size(370, 21);
            this.cb_TargetLang.TabIndex = 3;
            this.cb_TargetLang.SelectedIndexChanged += new System.EventHandler(this.Cb_TargetLang_SelectedIndexChanged);
            // 
            // cb_TargetDatabase
            // 
            this.cb_TargetDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_TargetDatabase.FormattingEnabled = true;
            this.cb_TargetDatabase.Location = new System.Drawing.Point(170, 90);
            this.cb_TargetDatabase.Name = "cb_TargetDatabase";
            this.cb_TargetDatabase.Size = new System.Drawing.Size(370, 21);
            this.cb_TargetDatabase.TabIndex = 4;
            this.cb_TargetDatabase.SelectedIndexChanged += new System.EventHandler(this.Cb_TargetDatabase_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Generate Mode :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Language :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Database :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Connection string :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 227);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Automatic reload project :";
            // 
            // cb_AutoReload
            // 
            this.cb_AutoReload.AutoSize = true;
            this.cb_AutoReload.Location = new System.Drawing.Point(170, 227);
            this.cb_AutoReload.Name = "cb_AutoReload";
            this.cb_AutoReload.Size = new System.Drawing.Size(15, 14);
            this.cb_AutoReload.TabIndex = 10;
            this.cb_AutoReload.UseVisualStyleBackColor = true;
            // 
            // GeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 312);
            this.Controls.Add(this.cb_AutoReload);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_TargetDatabase);
            this.Controls.Add(this.cb_TargetLang);
            this.Controls.Add(this.cb_GeneratorMode);
            this.Controls.Add(this.txt_ConnectionString);
            this.Controls.Add(this.btn_Generate);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GeneratorForm";
            this.Text = "Model Generator";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Generate;
        private System.Windows.Forms.RichTextBox txt_ConnectionString;
        private System.Windows.Forms.ComboBox cb_GeneratorMode;
        private System.Windows.Forms.ComboBox cb_TargetLang;
        private System.Windows.Forms.ComboBox cb_TargetDatabase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cb_AutoReload;
    }
}