using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASSemanticWeb
{

    public class SemanticWeb
    {
        private List<SemanticNode> _nodes;
        private List<SemanticArc> _arcs;
        public EventHandler OnChange;
        public event EventHandler Change;

        public List<SemanticNode> Nodes
        {
            get { return _nodes; }
            set
            {
                _nodes = value;
                if(Change!=null) Change(this, EventArgs.Empty);
            }
        }

        public List<SemanticArc> Arcs
        {
            get { return _arcs; }
            set
            {
                _arcs = value;
                if (Change != null) Change(this, EventArgs.Empty);
            }
        }

        public SemanticWeb()
        {
            Nodes = new List<SemanticNode>();
            Arcs = new List<SemanticArc>();
            
        }

        #region Работа с узлами и связями

        public void AddNode(string name, string comment, Point position)
        {
            int width = name.Length<15?name.Length:name.Length%15, height=20*(name.Length/15);
            SemanticNode node = new SemanticNode(name, comment, position, width, height,0,NodeType.Named);
            node.Change += node_Change;
            Nodes.Add(node);
            Change(this, EventArgs.Empty);
        }

        public void AddArc(string name, string comment, Color color, Bitmap image)
        {
            SemanticArc arc = new SemanticArc(name, comment, color, image);
            arc.Change += arc_Change;
            Arcs.Add(arc);
            Change(this, EventArgs.Empty);
        }

        public void RemoveNode(SemanticNode node)
        {
            if (!Nodes.Contains(node))
                return;
            node.Change -= node_Change;
            Nodes.Remove(node);
        }

        public void RemoveNodeAt(int index)
        {
            if (index < 0 || index >= Nodes.Count)
                return;
            Nodes[index].Change -= node_Change;
            Nodes.RemoveAt(index);
        }

        public void RemoveArc(SemanticArc arc)
        {
            if (!Arcs.Contains(arc))
                return;
            arc.Change -= arc_Change;
            Arcs.Remove(arc);
        }

        public void RemoveArcAt(int index)
        {
            if (index < 0 || index >= Arcs.Count)
                return;
            Arcs[index].Change -= arc_Change;
            Arcs.RemoveAt(index);
        }

        public void EditNode(SemanticNode node, string name, string comment)
        {
            if (!Nodes.Contains(node))
                return;
            if (!node.Name.ToUpper().Equals(name.ToUpper().Trim()))
                node.Name = name;
            if (!node.Comment.ToUpper().Equals(comment.ToUpper().Trim()))
                node.Comment = comment;
        }

        public void EditArc(SemanticArc arc, string name, string comment, Color color, Bitmap image)
        {
            if (!Arcs.Contains(arc))
                return;
            if (!arc.Name.ToUpper().Equals(name.ToUpper().Trim()))
                arc.Name = name;
            if (!arc.Comment.ToUpper().Equals(comment.ToUpper().Trim()))
                arc.Comment = comment;
            arc.Color = color;
            arc.Image = image;
        }

        public void AddArcForNode(SemanticNode node, SemanticArc arc, ArcDirection direction)
        {
            node.AddArc(direction, arc);
        }

        public void RemoveArcForNode(SemanticNode node, SemanticArc arc, ArcDirection direction)
        {
            node.RemoveArc(direction, arc);
        }

        #endregion

        void arc_Change(object sender, EventArgs e)
        {

        }

        void node_Change(object sender, EventArgs e)
        {

        }

        public void Draw(Graphics graphics)
        {
            
        }
    }
}
