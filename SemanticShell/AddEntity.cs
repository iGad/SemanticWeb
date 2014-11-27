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
    public partial class AddEntity : Form
    {
        public string EntityName { get; private set; }
        public string Description { get; private set; }
        public AddEntity()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NameTbx.Text))
            {
                MessageBox.Show("Имя не может быть пустым!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NameTbx.Focus();
                return;
            }
            EntityName = NameTbx.Text.Trim();
            Description = descriptionRtb.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
