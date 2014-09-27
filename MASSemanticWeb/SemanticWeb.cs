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
        private List<SemanticLink> _links;
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

        public List<SemanticLink> Links
        {
            get { return _links; }
            set
            {
                _links = value;
                if (Change != null) Change(this, EventArgs.Empty);
            }
        }

        public SemanticWeb()
        {
            Nodes = new List<SemanticNode>();
            Links = new List<SemanticLink>();
            
        }

        #region Работа с узлами и связями

        public void AddNode(string name, string comment, Point position)
        {
            int width = name.Length<15?name.Length:name.Length%15, height=20*(name.Length/15);
            SemanticNode node = new SemanticNode(name, comment, position, width, height);
            node.Change += node_Change;
            Nodes.Add(node);
            Change(this, EventArgs.Empty);
        }

        public void AddLink(string name, string comment, Color color, Bitmap image)
        {
            SemanticLink link = new SemanticLink(name, comment, color, image);
            link.Change += link_Change;
            Links.Add(link);
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

        public void RemoveLink(SemanticLink link)
        {
            if (!Links.Contains(link))
                return;
            link.Change -= link_Change;
            Links.Remove(link);
        }

        public void RemoveLinkAt(int index)
        {
            if (index < 0 || index >= Links.Count)
                return;
            Links[index].Change -= link_Change;
            Links.RemoveAt(index);
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

        public void EditLink(SemanticLink link, string name, string comment, Color color, Bitmap image)
        {
            if (!Links.Contains(link))
                return;
            if (!link.Name.ToUpper().Equals(name.ToUpper().Trim()))
                link.Name = name;
            if (!link.Comment.ToUpper().Equals(comment.ToUpper().Trim()))
                link.Comment = comment;
            link.Color = color;
            link.Image = image;
        }

        public void AddLinkForNode(SemanticNode node, SemanticLink link, LinkDirection direction)
        {
            node.AddLink(direction, link);
        }

        public void RemoveLinkForNode(SemanticNode node, SemanticLink link, LinkDirection direction)
        {
            node.RemoveLink(direction, link);
        }

        #endregion

        void link_Change(object sender, EventArgs e)
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
