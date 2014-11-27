using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MASSemanticWeb;

namespace SemanticShell
{
    public partial class AddArcBetweenNodes : Form
    {
        public int ArcIndex { get; private set; }
        public int FromNodeIndex { get; private set; }
        public int ToNodeIndex { get; private set; }
        public bool BothSideArc { get; private set; }
        private SemanticWeb semanticWeb;
        private List<int> toIndexes = new List<int>(); 
        public AddArcBetweenNodes(SemanticWeb semanticWeb)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            
            this.semanticWeb = semanticWeb;
            var nodes = semanticWeb.Nodes.Select(node=>node.Name);
            foreach (string node in nodes)
            {
                fromCmbx.Items.Add(node);
                //toCmbx.Items.Add(node);
            }
            var arcs = semanticWeb.Arcs.Select(arc => arc.Name);
            foreach (string arc in arcs)
            {
                arcCmbx.Items.Add(arc);
            }
            if (arcCmbx.Items.Count > 0)
                arcCmbx.SelectedIndex = 0;
            if (fromCmbx.Items.Count > 1)
            {
                fromCmbx.SelectedIndex = 0;
                //toCmbx.SelectedIndex = 1;
            }

        }

        private void AddArc_Load(object sender, EventArgs e)
        {

        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (arcCmbx.SelectedIndex < 0)
            {
                MessageBox.Show("Сначала необходимо добавить связь", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (fromCmbx.SelectedIndex < 0 || toCmbx.SelectedIndex < 0)
            {
                MessageBox.Show("Сначала необходимо добавить сущности", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (fromCmbx.SelectedIndex == toCmbx.SelectedIndex)
            {
                MessageBox.Show("Нельзя создать связь между одной и той же сущностью", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fromCmbx.Focus();
                return;
            }
            ArcIndex = arcCmbx.SelectedIndex;
            BothSideArc = false;
            FromNodeIndex = fromCmbx.SelectedIndex;
            ToNodeIndex = toIndexes[toCmbx.SelectedIndex];
            this.DialogResult = DialogResult.OK;
            
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fromCmbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fromCmbx.SelectedIndex < 0)
                return;
            toCmbx.Items.Clear();
            for (int i = 0; i < semanticWeb.Nodes.Count; i++)
            {
                if (!semanticWeb.Nodes[fromCmbx.SelectedIndex].OutArcs.ContainsKey(semanticWeb.Nodes[i]) &&
                    !semanticWeb.Nodes[fromCmbx.SelectedIndex].InArcs.ContainsKey(semanticWeb.Nodes[i])
                    && semanticWeb.Nodes[i] != semanticWeb.Nodes[fromCmbx.SelectedIndex])
                {
                    toCmbx.Items.Add(semanticWeb.Nodes[i].Name);
                    toIndexes.Add(i);
                }
            }
            if (toCmbx.Items.Count > 0)
                toCmbx.SelectedIndex = 0;
        }
    }
}
