using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace MASSemanticWeb
{
    public enum ArcDirection
    {
        Inner,
        Outter,
        Both
    }

    public enum NodeType
    {
        Named,
        System
    }

    //TODO: подумать насчет признаков (например является мета-объектом или расширением мета-объекта
    public enum NodeSign
    {
        Meta,
        None
    }

    public class SemanticNode
    {
        //TODO: как расположена система координат?
        private string _name;
        private string _comment;
        public EventHandler OnChange;
        public event EventHandler Change;
        public event EventHandler PositionChange;
        //Так как связь между двумя узлами может быть только одна, исользуем узел как уникальный ключ
        private Dictionary<SemanticNode,SemanticArc> _inArcs;
        private Dictionary<SemanticNode, SemanticArc> _outArcs;
        private NodeType _type;
        private NodeSign _sign;
        private Point _position;//местоположение левого верхнего угла
        private readonly int _id;

        #region Свойства
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (OnChange != null) Change(this, EventArgs.Empty); //OnChange(this, EventArgs.Empty);
            }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                if (Change != null) Change(this, EventArgs.Empty);
            }
        }

        public Dictionary<SemanticNode, SemanticArc> InArcs
        {
            get { return _inArcs; }
            private set
            {
                _inArcs = value;
            }
        }

        public Dictionary<SemanticNode, SemanticArc> OutArcs
        {
            get { return _outArcs; }
            private set
            {
                _outArcs = value;

            }
        }

        public Point Position
        {
            get { return _position; }
            set
            {
                _position = value;
                PositionChange(this, EventArgs.Empty);
            }
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Id
        {
            get { return _id; }
        }

        public NodeType Type
        {
            get { return _type; }
        }

        public NodeSign Sign
        {
            get { return _sign; }
            set { _sign = value; }
        }

        //изменение позиции не считается изменением
        #endregion

        public SemanticNode(string name, string comment, Point position, int width, int height, int id, NodeType type)
        {
            _name = name;
            _comment = comment;
            _position = position;
            Width = width;
            Height = height;
            _inArcs = new Dictionary<SemanticNode, SemanticArc>();
            _outArcs = new Dictionary<SemanticNode, SemanticArc>();
            _id = id;
            _type = type;
            _sign = NodeSign.None;
            
        }

        public SemanticNode(string name, string comment, Point position, int width, int height, int id, NodeType type, NodeSign sign)
        {
            _name = name;
            _comment = comment;
            _position = position;
            Width = width;
            Height = height;
            _inArcs = new Dictionary<SemanticNode, SemanticArc>();
            _outArcs = new Dictionary<SemanticNode, SemanticArc>();
            _id = id;
            _type = type;
            _sign = sign;

        }

        public void AddArc(ArcDirection direction, SemanticArc arc, SemanticNode node)
        {
            if (direction != ArcDirection.Outter)
                _inArcs.Add(node, arc);
            if (direction != ArcDirection.Inner)
                _outArcs.Add(node, arc);
            if (Change != null) Change(this, EventArgs.Empty);
        }

        public void RemoveArc(ArcDirection direction, SemanticNode node)
        {
            if (direction != ArcDirection.Inner)
            {
                if (_outArcs.ContainsKey(node))
                    _outArcs.Remove(node);
            }
            if (direction != ArcDirection.Outter)
            {
                if (_inArcs.ContainsKey(node))
                    _inArcs.Remove(node);
            }
        }

        public void Dispose()
        {
            _outArcs = null;
            _inArcs = null;

        }
    }



    public class SemanticArc
    {
        private readonly int _id;
        private string _name;
        private string _comment;
        public EventHandler OnChange;
        public event EventHandler Change;
        private Color _color;
        private Bitmap _image;//изображение, которое будет отображаться на связи

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if(Change != null) Change(this, EventArgs.Empty);
            }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                if(Change != null) Change(this, EventArgs.Empty);
            }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                if(Change != null) Change(this, EventArgs.Empty);
            }
        }

        public Bitmap Image
        {
            get { return _image; }
            set
            {
                _image = value;
                if(Change != null) Change(this, EventArgs.Empty);
            }
        }

        public int Id
        {
            get { return _id; }
        }

        public SemanticArc(int id, string name, string comment, Color color, Bitmap image)
        {
            this.Comment = comment;
            this.Name = name;
            this.Color = color;
            this.Image = image;
            this._id = id;
        }

        public static SemanticArc CreateNew(string name, string comment, Color color, Bitmap image)
        {
            return new SemanticArc(name, comment, color, image);
        }
    }
}
