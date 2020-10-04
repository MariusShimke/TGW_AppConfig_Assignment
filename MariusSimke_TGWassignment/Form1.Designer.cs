namespace MariusSimke_TGWassignment
{
    partial class Form1
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
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lblResult = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblValidationMessage = new System.Windows.Forms.Label();
            this.chbDisplayAll = new System.Windows.Forms.CheckBox();
            this.chbAddNew = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(673, 395);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFile.TabIndex = 0;
            this.btnSelectFile.Text = "Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "txt";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            this.openFileDialog1.FilterIndex = 2;
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(54, 83);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(23, 13);
            this.lblResult.TabIndex = 2;
            this.lblResult.Text = "null";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(54, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(213, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Most Recent Config Settings";
            // 
            // lblValidationMessage
            // 
            this.lblValidationMessage.ForeColor = System.Drawing.Color.Red;
            this.lblValidationMessage.Location = new System.Drawing.Point(302, 58);
            this.lblValidationMessage.Name = "lblValidationMessage";
            this.lblValidationMessage.Size = new System.Drawing.Size(446, 320);
            this.lblValidationMessage.TabIndex = 7;
            // 
            // chbDisplayAll
            // 
            this.chbDisplayAll.AutoSize = true;
            this.chbDisplayAll.Location = new System.Drawing.Point(390, 399);
            this.chbDisplayAll.Name = "chbDisplayAll";
            this.chbDisplayAll.Size = new System.Drawing.Size(74, 17);
            this.chbDisplayAll.TabIndex = 9;
            this.chbDisplayAll.Text = "Display All";
            this.chbDisplayAll.UseVisualStyleBackColor = true;
            this.chbDisplayAll.CheckedChanged += new System.EventHandler(this.chbDisplayAll_CheckedChanged);
            // 
            // chbAddNew
            // 
            this.chbAddNew.AutoSize = true;
            this.chbAddNew.Location = new System.Drawing.Point(531, 399);
            this.chbAddNew.Name = "chbAddNew";
            this.chbAddNew.Size = new System.Drawing.Size(70, 17);
            this.chbAddNew.TabIndex = 10;
            this.chbAddNew.Text = "Add New";
            this.chbAddNew.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chbAddNew);
            this.Controls.Add(this.chbDisplayAll);
            this.Controls.Add(this.lblValidationMessage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnSelectFile);
            this.Name = "Form1";
            this.Text = "Configuration Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblValidationMessage;
        private System.Windows.Forms.CheckBox chbDisplayAll;
        private System.Windows.Forms.CheckBox chbAddNew;
    }
}

