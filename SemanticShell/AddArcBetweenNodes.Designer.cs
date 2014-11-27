namespace SemanticShell
{
    partial class AddArcBetweenNodes
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
            this.cancelBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.fromCmbx = new System.Windows.Forms.ComboBox();
            this.toCmbx = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.arcCmbx = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cancelBtn
            // 
            this.cancelBtn.BackColor = System.Drawing.SystemColors.Control;
            this.cancelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cancelBtn.Location = new System.Drawing.Point(173, 115);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(126, 47);
            this.cancelBtn.TabIndex = 10;
            this.cancelBtn.Text = "Отмена";
            this.cancelBtn.UseVisualStyleBackColor = false;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.BackColor = System.Drawing.SystemColors.Control;
            this.addBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.addBtn.Location = new System.Drawing.Point(16, 115);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(126, 47);
            this.addBtn.TabIndex = 9;
            this.addBtn.Text = "Добавить";
            this.addBtn.UseVisualStyleBackColor = false;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label4.Location = new System.Drawing.Point(12, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 24);
            this.label4.TabIndex = 11;
            this.label4.Text = "Откуда:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label5.Location = new System.Drawing.Point(13, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 24);
            this.label5.TabIndex = 11;
            this.label5.Text = "Куда:";
            // 
            // fromCmbx
            // 
            this.fromCmbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.fromCmbx.FormattingEnabled = true;
            this.fromCmbx.Location = new System.Drawing.Point(115, 43);
            this.fromCmbx.Name = "fromCmbx";
            this.fromCmbx.Size = new System.Drawing.Size(184, 28);
            this.fromCmbx.TabIndex = 4;
            this.fromCmbx.SelectedIndexChanged += new System.EventHandler(this.fromCmbx_SelectedIndexChanged);
            // 
            // toCmbx
            // 
            this.toCmbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.toCmbx.FormattingEnabled = true;
            this.toCmbx.Location = new System.Drawing.Point(115, 77);
            this.toCmbx.Name = "toCmbx";
            this.toCmbx.Size = new System.Drawing.Size(184, 28);
            this.toCmbx.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 24);
            this.label1.TabIndex = 11;
            this.label1.Text = "Связь:";
            // 
            // arcCmbx
            // 
            this.arcCmbx.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.arcCmbx.FormattingEnabled = true;
            this.arcCmbx.Location = new System.Drawing.Point(115, 9);
            this.arcCmbx.Name = "arcCmbx";
            this.arcCmbx.Size = new System.Drawing.Size(184, 28);
            this.arcCmbx.TabIndex = 4;
            // 
            // AddArcBetweenNodes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(314, 171);
            this.ControlBox = false;
            this.Controls.Add(this.toCmbx);
            this.Controls.Add(this.arcCmbx);
            this.Controls.Add(this.fromCmbx);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.addBtn);
            this.Name = "AddArcBetweenNodes";
            this.Text = "Добавление связи";
            this.Load += new System.EventHandler(this.AddArc_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox fromCmbx;
        private System.Windows.Forms.ComboBox toCmbx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox arcCmbx;
    }
}