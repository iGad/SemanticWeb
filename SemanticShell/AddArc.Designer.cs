namespace SemanticShell
{
    partial class AddArc
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
            this.selectImageBtn = new System.Windows.Forms.Button();
            this.selectColorBtn = new System.Windows.Forms.Button();
            this.imagePbx = new System.Windows.Forms.PictureBox();
            this.colorPbx = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.descriptionRtb = new System.Windows.Forms.RichTextBox();
            this.nameTbx = new System.Windows.Forms.TextBox();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.imagePbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPbx)).BeginInit();
            this.SuspendLayout();
            // 
            // selectImageBtn
            // 
            this.selectImageBtn.BackColor = System.Drawing.SystemColors.Control;
            this.selectImageBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.selectImageBtn.Location = new System.Drawing.Point(308, 178);
            this.selectImageBtn.Name = "selectImageBtn";
            this.selectImageBtn.Size = new System.Drawing.Size(75, 25);
            this.selectImageBtn.TabIndex = 19;
            this.selectImageBtn.Text = "Выбрать";
            this.selectImageBtn.UseVisualStyleBackColor = false;
            this.selectImageBtn.Click += new System.EventHandler(this.selectImageBtn_Click);
            // 
            // selectColorBtn
            // 
            this.selectColorBtn.BackColor = System.Drawing.SystemColors.Control;
            this.selectColorBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.selectColorBtn.Location = new System.Drawing.Point(308, 149);
            this.selectColorBtn.Name = "selectColorBtn";
            this.selectColorBtn.Size = new System.Drawing.Size(75, 25);
            this.selectColorBtn.TabIndex = 13;
            this.selectColorBtn.Text = "Выбрать";
            this.selectColorBtn.UseVisualStyleBackColor = false;
            this.selectColorBtn.Click += new System.EventHandler(this.selectColorBtn_Click);
            // 
            // imagePbx
            // 
            this.imagePbx.BackColor = System.Drawing.Color.White;
            this.imagePbx.Location = new System.Drawing.Point(120, 178);
            this.imagePbx.Name = "imagePbx";
            this.imagePbx.Size = new System.Drawing.Size(184, 93);
            this.imagePbx.TabIndex = 29;
            this.imagePbx.TabStop = false;
            // 
            // colorPbx
            // 
            this.colorPbx.BackColor = System.Drawing.Color.Black;
            this.colorPbx.Location = new System.Drawing.Point(120, 149);
            this.colorPbx.Name = "colorPbx";
            this.colorPbx.Size = new System.Drawing.Size(184, 23);
            this.colorPbx.TabIndex = 28;
            this.colorPbx.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label6.Location = new System.Drawing.Point(18, 177);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 17);
            this.label6.TabIndex = 24;
            this.label6.Text = "Изображение";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label3.Location = new System.Drawing.Point(17, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 24);
            this.label3.TabIndex = 26;
            this.label3.Text = "Цвет:";
            // 
            // descriptionRtb
            // 
            this.descriptionRtb.Location = new System.Drawing.Point(120, 41);
            this.descriptionRtb.Name = "descriptionRtb";
            this.descriptionRtb.Size = new System.Drawing.Size(367, 105);
            this.descriptionRtb.TabIndex = 20;
            this.descriptionRtb.Text = "";
            // 
            // nameTbx
            // 
            this.nameTbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.nameTbx.Location = new System.Drawing.Point(120, 9);
            this.nameTbx.Name = "nameTbx";
            this.nameTbx.Size = new System.Drawing.Size(367, 26);
            this.nameTbx.TabIndex = 21;
            // 
            // cancelBtn
            // 
            this.cancelBtn.BackColor = System.Drawing.SystemColors.Control;
            this.cancelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cancelBtn.Location = new System.Drawing.Point(361, 277);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(126, 47);
            this.cancelBtn.TabIndex = 23;
            this.cancelBtn.Text = "Отмена";
            this.cancelBtn.UseVisualStyleBackColor = false;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.BackColor = System.Drawing.SystemColors.Control;
            this.addBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.addBtn.Location = new System.Drawing.Point(120, 277);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(126, 47);
            this.addBtn.TabIndex = 22;
            this.addBtn.Text = "Добавить";
            this.addBtn.UseVisualStyleBackColor = false;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 24);
            this.label2.TabIndex = 16;
            this.label2.Text = "Описание:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 24);
            this.label1.TabIndex = 18;
            this.label1.Text = "Название:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Изображения .png|*.png";
            // 
            // AddArc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(498, 332);
            this.ControlBox = false;
            this.Controls.Add(this.selectImageBtn);
            this.Controls.Add(this.selectColorBtn);
            this.Controls.Add(this.imagePbx);
            this.Controls.Add(this.colorPbx);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.descriptionRtb);
            this.Controls.Add(this.nameTbx);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AddArc";
            this.Text = "Добавление связи";
            ((System.ComponentModel.ISupportInitialize)(this.imagePbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorPbx)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button selectImageBtn;
        private System.Windows.Forms.Button selectColorBtn;
        private System.Windows.Forms.PictureBox imagePbx;
        private System.Windows.Forms.PictureBox colorPbx;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox descriptionRtb;
        private System.Windows.Forms.TextBox nameTbx;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}