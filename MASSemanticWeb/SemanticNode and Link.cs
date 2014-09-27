using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace MASSemanticWeb
{
    public enum LinkDirection
    {
        Inner,
        Outter,
        Both
    }

    public class SemanticNode
    {
        private string _name;
        private string _comment;
        public EventHandler OnChange;
        public event EventHandler Change;
        private List<SemanticLink> _inLinks;
        private List<SemanticLink> _outLinks;
        private Point _position;//местоположение левого верхнего угла
        private int _width;
        private int _height;

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
                if(Change != null) Change(this, EventArgs.Empty);
            }
        }

        public List<SemanticLink> InLinks
        {
            get { return _inLinks; }
            private set
            {
                _inLinks = value;
            }
        }

        public List<SemanticLink> OutLinks
        {
            get { return _outLinks; }
            private set
            {
                _outLinks = value;
                
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

//изменение позиции не считается изменением
        #endregion

        public SemanticNode(string name, string comment, Point position, int width, int height)
        {
            _name = name;
            _comment = comment;
            _position = position;
            _width = width;
            _height = height;
            _inLinks = new List<SemanticLink>();
            _outLinks = new List<SemanticLink>();
        }

        public void AddLink(LinkDirection direction, SemanticLink link)
        {
            if (direction != LinkDirection.Outter)
                _inLinks.Add(link);
            if(direction!= LinkDirection.Inner)
                _outLinks.Add(link);
            if(Change != null) Change(this, EventArgs.Empty);
        }

        public void RemoveLink(LinkDirection direction, SemanticLink link)
        {
            if (direction != LinkDirection.Inner)
            {
                if (_outLinks.Contains(link))
                    _outLinks.Remove(link);
            }
            if (direction != LinkDirection.Outter)
            {
                if (_inLinks.Contains(link))
                    _inLinks.Remove(link);
            }
        }

        public void Dispose()
        {
            _outLinks = null;
            _inLinks = null;

        }
    }

    public class SemanticLink
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

        public SemanticLink(string name, string comment, Color color, Bitmap image)
        {
            this.Comment = comment;
            this._name = name;
            this.Color = color;
            this.Image = image;
        }

        public static SemanticLink CreateNew(string name, string comment, Color color, Bitmap image)
        {
            return new SemanticLink(name, comment, color, image);
        }
    }
}
