using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MASSemanticWeb;

namespace MASSemanticWeb
{
    public class DrawThread
    {
        private int _width, _height;
        private double _attractionConstant = 0.1;		// spring constant
        private double _repulsionConstant = 10000;	// charge constant

        private double _damping = 0.1;
        private int _springLength = 300;
        private int _maxIterations = 500;
        private SemanticWeb _semanticWeb;
        public bool IsAlive { get; private set; }
        private bool deterministic;
        private Thread workThread;

        public DrawThread(int width, int height, double attraction, double repulsion, double damping, int spring,
                          int iterations, SemanticWeb semanticWeb, bool deterministic)
        {
            _semanticWeb = semanticWeb;
            _width = width;
            _height = height;
            _attractionConstant = attraction;
            _repulsionConstant = repulsion;
            _damping = damping;
            _springLength = spring;
            _maxIterations = iterations;
            this.deterministic = deterministic;
            workThread = new Thread(Work);
        }

        public bool Run()
        {
            if (!workThread.IsAlive)
            {
                IsAlive = true;
                workThread.Start();
                return true;
            }
            else
                return false;
            
        }

        private void Work()
        {
            Random rnd = deterministic ? new Random(0) : new Random();

            // Копируем узлы в массив и задаем случайные координаты
            NodeLayoutInfo[] layout = new NodeLayoutInfo[_semanticWeb.Nodes.Count];
            for (int i = 0; i < _semanticWeb.Nodes.Count; i++)
            {
                layout[i] = new NodeLayoutInfo(_semanticWeb.Nodes[i], new Vector(), Point.Empty);
                layout[i].Node.Position = new Point(rnd.Next(0, 500), rnd.Next(0, 500));
            }

            int stopCount = 0;
            int iterations = 0;

            while (IsAlive)
            {
                double totalDisplacement = 0;

                for (int i = 0; i < layout.Length; i++)
                {
                    NodeLayoutInfo current = layout[i];

                    // выражаем текущее положение узла в виде вектора , относительно начала
                    Vector currentPosition = new Vector(CalcDistance(Point.Empty, current.Node.Position), GetBearingAngle(Point.Empty, current.Node.Position));
                    Vector netForce = new Vector(0, 0);

                    // Определяем отталкивание
                    foreach (SemanticNode other in _semanticWeb.Nodes)
                    {
                        if (other.Name != current.Node.Name) netForce += CalcRepulsionForce(current.Node, other);
                    }

                    // Опрделяем притяжение
                    foreach (SemanticNode child in current.Node.OutArcs.Keys)
                    {
                        netForce += CalcAttractionForce(current.Node, child, _springLength);
                    }
                    foreach (SemanticNode parent in _semanticWeb.Nodes)
                    {
                        if (parent.OutArcs.Keys.Contains(current.Node)) netForce += CalcAttractionForce(current.Node, parent, _springLength);
                    }

                    current.Velocity = (current.Velocity + netForce) * _damping;

                    current.NextPosition = (currentPosition + current.Velocity).ToPoint();
                }

                // Передвигаем узлы
                for (int i = 0; i < layout.Length; i++)
                {
                    NodeLayoutInfo current = layout[i];

                    totalDisplacement += CalcDistance(current.Node.Position, current.NextPosition);
                    current.Node.Position = current.NextPosition;
                }

                iterations++;
                if (totalDisplacement < 10) stopCount++;
                IsAlive = stopCount <= 15 && iterations <= _maxIterations;
                Thread.Sleep(2000);
            }

            // Центрируем сеть
            //Rectangle logicalBounds = GetDiagramBounds();
            //Point midPoint = new Point(logicalBounds.X + (logicalBounds.Width / 2), logicalBounds.Y + (logicalBounds.Height / 2));

            //foreach (SemanticNode node in _semanticWeb.Nodes)
            //{
            //    node.Position -= (Size)midPoint;
            //}
        }
        private Rectangle GetDiagramBounds()
        {
            int minX = Int32.MaxValue, minY = Int32.MaxValue;
            int maxX = Int32.MinValue, maxY = Int32.MinValue;
            foreach (SemanticNode node in _semanticWeb.Nodes)
            {
                if (node.Position.X < minX) minX = node.Position.X;
                if (node.Position.X > maxX) maxX = node.Position.X;
                if (node.Position.Y < minY) minY = node.Position.Y;
                if (node.Position.Y > maxY) maxY = node.Position.Y;
            }

            return Rectangle.FromLTRB(minX, minY, maxX, maxY);
        }

        /// <summary>
        /// Вычисление силы между точками
        /// </summary>
        /// <param name="x">Точка на которую направлена сила</param>
        /// <param name="y"> Точка создающая силу</param>
        /// <returns>Вектор, представляющий силу</returns>
        private Vector CalcRepulsionForce(SemanticNode x, SemanticNode y)
        {
            int proximity = Math.Max(CalcDistance(x.Position, y.Position), 1);

            // Coulomb's Law: F = k(Qq/r^2)
            double force = -(_repulsionConstant / Math.Pow(proximity, 2));
            double angle = GetBearingAngle(x.Position, y.Position);

            return new Vector(force, angle);
        }

        /// <summary>
        /// Вычисляем угол
        /// </summary>
        /// <param name="start">Начальная точка</param>
        /// <param name="end">Конечная точка</param>
        /// <returns>Угол в градусах</returns>
        private double GetBearingAngle(Point start, Point end)
        {
            Point half = new Point(start.X + ((end.X - start.X) / 2), start.Y + ((end.Y - start.Y) / 2));

            double diffX = (double)(half.X - start.X);
            double diffY = (double)(half.Y - start.Y);

            if (diffX == 0) diffX = 0.001;
            if (diffY == 0) diffY = 0.001;

            double angle;
            if (Math.Abs(diffX) > Math.Abs(diffY))
            {
                angle = Math.Tanh(diffY / diffX) * (180.0 / Math.PI);
                if (((diffX < 0) && (diffY > 0)) || ((diffX < 0) && (diffY < 0))) angle += 180;
            }
            else
            {
                angle = Math.Tanh(diffX / diffY) * (180.0 / Math.PI);
                if (((diffY < 0) && (diffX > 0)) || ((diffY < 0) && (diffX < 0))) angle += 180;
                angle = (180 - (angle + 90));
            }

            return angle;
        }

        /// <summary>
        /// Вычислени силы
        /// </summary>
        /// <param name="x">Приемник</param>
        /// <param name="y">Источник</param>
        /// <param name="springLength">Длина в пикселях</param>
        /// <returns>Вектор силы</returns>
        private Vector CalcAttractionForce(SemanticNode x, SemanticNode y, double springLength)
        {
            int proximity = Math.Max(CalcDistance(x.Position, y.Position), 1);

            // Hooke's Law: F = -kx
            double force = _attractionConstant * Math.Max(proximity - springLength, 0);
            double angle = GetBearingAngle(x.Position, y.Position);

            return new Vector(force, angle);
        }

        /// <summary>
        /// Вычисление расстояния между двумя точками
        /// </summary>
        /// <param name="a">Первая точка</param>
        /// <param name="b">Вторая точка</param>
        /// <returns>Расстояние в точках</returns>
        public static int CalcDistance(Point a, Point b)
        {
            double xDist = (a.X - b.X);
            double yDist = (a.Y - b.Y);
            return (int)Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
        }

        public class NodeLayoutInfo
        {

            public SemanticNode Node;			// Сслка на узел
            public Vector Velocity;		// Текущая скорость узлы
            public Point NextPosition;	// Позиция узла после иттерации

            public NodeLayoutInfo(SemanticNode node, Vector velocity, Point nextPosition)
            {
                Node = node;
                Velocity = velocity;
                NextPosition = nextPosition;
            }
        }
    }
}
