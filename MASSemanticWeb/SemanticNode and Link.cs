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

    public class SemanticNode
    {
        private string _name;
        private string _comment;
        public EventHandler OnChange;
        public event EventHandler Change;
        private List<SemanticArc> _inArcs;
        private List<SemanticArc> _outArcs;
        private NodeType _type;
        private Point _position;//местоположение левого верхнего угла
        private int _width;
        private int _height;
        private int _id;

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

        public List<SemanticArc> InArcs
        {
            get { return _inArcs; }
            private set
            {
                _inArcs = value;
            }
        }

        public List<SemanticArc> OutArcs
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
            set { _position = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public int Id
        {
            get { return _id; }
        }

        public NodeType Type
        {
            get { return _type; }
        }

        //изменение позиции не считается изменением
        #endregion

        public SemanticNode(string name, string comment, Point position, int width, int height, int id, NodeType type)
        {
            _name = name;
            _comment = comment;
            _position = position;
            _width = width;
            _height = height;
            _inArcs = new List<SemanticArc>();
            _outArcs = new List<SemanticArc>();
            _id = id;
            _type = type;
        }

        public void AddArc(ArcDirection direction, SemanticArc arc)
        {
            if (direction != ArcDirection.Outter)
                _inArcs.Add(arc);
            if (direction != ArcDirection.Inner)
                _outArcs.Add(arc);
            if (Change != null) Change(this, EventArgs.Empty);
        }

        public void RemoveArc(ArcDirection direction, SemanticArc arc)
        {
            if (direction != ArcDirection.Inner)
            {
                if (_outArcs.Contains(arc))
                    _outArcs.Remove(arc);
            }
            if (direction != ArcDirection.Outter)
            {
                if (_inArcs.Contains(arc))
                    _inArcs.Remove(arc);
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
        private string _name;
        private string _comment;
        public EventHandler OnChange;
        public event EventHandler Change;
        private Color _color;
        private Bitmap _image;

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

        public SemanticArc(string name, string comment, Color color, Bitmap image)
        {
            this.Comment = comment;
            this._name = name;
            this.Color = color;
            this.Image = image;
        }

        public static SemanticArc CreateNew(string name, string comment, Color color, Bitmap image)
        {
            return new SemanticArc(name, comment, color, image);
        }
    }
}
