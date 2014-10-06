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
        //по умолчанию, при создании сем. сети сразу создаем системные узлы и связи
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

        public SemanticNode this[int id] {
            get
            {
                return Nodes.Find(node => node.Id == id);
  
            } 
        }

        public List<SemanticNode> this[string name]
        {
            get
            {
                return Nodes.FindAll(node => node.Name.ToLower() == name.ToLower().Trim());
            }
        }

        public SemanticWeb()
        {
            Nodes = new List<SemanticNode>();
            Arcs = new List<SemanticArc>();
            AddNode(0, "#System", "Основная системная вершина", new Point(0, 0));
            AddArc(0,"is_a", "Связь класс-подкласс", Color.Black, null);
        }

        #region Работа с узлами и связями

        public void AddNode(string name, string comment, Point position)
        {
            int width = name.Length<15?name.Length:name.Length%15, height=20*(name.Length/15);
            SemanticNode node = new SemanticNode(name, comment, position, width, height, 0, NodeType.Named);
            node.Change += node_Change;
            Nodes.Add(node);
            Change(this, EventArgs.Empty);
        }

        public bool AddNode(int id,string name, string comment, Point position)
        {
            if (Nodes.Find(n => n.Id == id) != null)
                return false;
            int width = name.Length < 15 ? name.Length : name.Length % 15, height = 20 * (name.Length / 15);
            SemanticNode node = new SemanticNode(name, comment, position, width, height, id, NodeType.Named);
            node.Change += node_Change;
            Nodes.Add(node);
            Change(this, EventArgs.Empty);
            return true;
        }

        public void AddArc(int id,string name, string comment, Color color, Bitmap image)
        {
            SemanticArc arc = new SemanticArc(id, name, comment, color, image);
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

        public void AddArcBetweenNodes(SemanticNode fromNode, SemanticNode toNode, SemanticArc arc,
                                       bool bothSide)
        {
            if (bothSide)
            {
                fromNode.AddArc(ArcDirection.Both, arc, toNode);
                toNode.AddArc(ArcDirection.Both, arc, fromNode);
            }
            else
            {

                fromNode.AddArc(ArcDirection.Outter, arc, toNode);
                toNode.AddArc(ArcDirection.Inner, arc, fromNode);
            }
        }

        #endregion

        /// <summary>
        ///  Метод находит все узлы, из которых выходит дуга в переданный узел. В идеале поиск должен всегда заканчиваться
        /// узлом #System
        /// </summary>
        /// <param name="endNode">Узел, для которого выполняется поиск</param>
        /// <param name="transitivity"> Учитывать транзитивность</param>
        /// <returns>Список найденных узлов или null, если в узел не входит ни одна дуга</returns>
        public List<SemanticNode> GetAllInnerNodes(SemanticNode endNode, bool transitivity)
        {
            if (endNode.InArcs.Count == 0)
                return null;
            List<SemanticNode> result = new List<SemanticNode>();
            foreach (SemanticNode innerNode in endNode.InArcs.Keys)
            {
                if (!transitivity)
                    result.Add(innerNode);
                else
                {
                    if (innerNode == Nodes[0])
                        //системная вершина, с которой связаны все вершины, то же самое что Nodes["#System"]. 
                        result.Add(innerNode);
                    else
                    {
                        IEnumerable<SemanticNode> temp = GetAllInnerNodes(innerNode, true);
                        if (temp != null)
                            result.AddRange(temp);

                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Метод поиска пересечения между множествами входящих в node1 и node2 вершин
        /// </summary>
        /// <param name="node1">Первый узел</param>
        /// <param name="node2">Второй узел</param>
        /// <param name="transitivity">Учитывать транзиитивность</param>
        /// <returns>Список узлов, которые входят в обе множества, в идеале всегда должен содержать узел #System</returns>
        public List<SemanticNode> Intersect(SemanticNode node1, SemanticNode node2, bool transitivity)
        {
            IEnumerable<SemanticNode> set1 = GetAllInnerNodes(node1, transitivity);
            IEnumerable<SemanticNode> set2 = GetAllInnerNodes(node2, transitivity);
            return set1.Intersect(set2).ToList();
        }

        /// <summary>
        /// Метод объединения двух множеств входящих в node1 и node2 вершин
        /// </summary>
        /// <param name="node1">Первый узел</param>
        /// <param name="node2">Второй узел</param>
        /// <param name="transitivity">Учитывать транзиитивность</param>
        /// <returns>Список узлов, которые входят хотя бы в одно множество</returns>
        public List<SemanticNode> Union(SemanticNode node1, SemanticNode node2, bool transitivity)
        {
            IEnumerable<SemanticNode> set1 = GetAllInnerNodes(node1, transitivity);
            IEnumerable<SemanticNode> set2 = GetAllInnerNodes(node2, transitivity);
            return set1.Union(set2).ToList();
        }

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
