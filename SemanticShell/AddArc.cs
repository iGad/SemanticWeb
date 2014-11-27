using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SemanticShell
{
    public partial class AddArc : Form
    {
        public string ArcName { get; private set; }
        public string Comment { get; private set; }
        public Color Color { get; private set; }
        public Image Image { get; private set; }
        public AddArc()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ArcName))
            {
                MessageBox.Show("Введите название связи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                nameTbx.Focus();
                return;
            }
            ArcName = nameTbx.Text.Trim();
            Comment = descriptionRtb.Text.Trim();
            Color = colorPbx.BackColor;
            Image = imagePbx.Image;
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void selectColorBtn_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                colorPbx.BackColor = colorDialog1.Color;
            }
        }

        private void selectImageBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imagePbx.Load(openFileDialog1.FileName);
            }
        }

    }
}
