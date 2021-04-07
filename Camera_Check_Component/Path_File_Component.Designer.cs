namespace Camera_Check_Component
{
    partial class Path_File_Component
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Path_File_Component));
            this.Open_Dialog_btn = new System.Windows.Forms.Button();
            this.Saving_btn = new System.Windows.Forms.Button();
            this.Cancel_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBox_PathFile = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Open_Dialog_btn
            // 
            this.Open_Dialog_btn.Location = new System.Drawing.Point(571, 16);
            this.Open_Dialog_btn.Name = "Open_Dialog_btn";
            this.Open_Dialog_btn.Size = new System.Drawing.Size(40, 23);
            this.Open_Dialog_btn.TabIndex = 0;
            this.Open_Dialog_btn.Text = "...";
            this.Open_Dialog_btn.UseVisualStyleBackColor = true;
            this.Open_Dialog_btn.Click += new System.EventHandler(this.Open_Dialog_btn_Click);
            // 
            // Saving_btn
            // 
            this.Saving_btn.Location = new System.Drawing.Point(160, 53);
            this.Saving_btn.Name = "Saving_btn";
            this.Saving_btn.Size = new System.Drawing.Size(122, 37);
            this.Saving_btn.TabIndex = 1;
            this.Saving_btn.Text = "SAVE";
            this.Saving_btn.UseVisualStyleBackColor = true;
            this.Saving_btn.Click += new System.EventHandler(this.Saving_btn_Click);
            // 
            // Cancel_btn
            // 
            this.Cancel_btn.Location = new System.Drawing.Point(347, 53);
            this.Cancel_btn.Name = "Cancel_btn";
            this.Cancel_btn.Size = new System.Drawing.Size(122, 37);
            this.Cancel_btn.TabIndex = 2;
            this.Cancel_btn.Text = "CANCEL";
            this.Cancel_btn.UseVisualStyleBackColor = true;
            this.Cancel_btn.Click += new System.EventHandler(this.Cancel_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Path File :";
            // 
            // TextBox_PathFile
            // 
            this.TextBox_PathFile.Location = new System.Drawing.Point(98, 18);
            this.TextBox_PathFile.Name = "TextBox_PathFile";
            this.TextBox_PathFile.Size = new System.Drawing.Size(454, 20);
            this.TextBox_PathFile.TabIndex = 4;
            // 
            // Path_File_Component
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 110);
            this.Controls.Add(this.TextBox_PathFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancel_btn);
            this.Controls.Add(this.Saving_btn);
            this.Controls.Add(this.Open_Dialog_btn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Path_File_Component";
            this.Text = "Path_File_Component";
            this.Load += new System.EventHandler(this.Path_File_Component_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Open_Dialog_btn;
        private System.Windows.Forms.Button Saving_btn;
        private System.Windows.Forms.Button Cancel_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBox_PathFile;
    }
}