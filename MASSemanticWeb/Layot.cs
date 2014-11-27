using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASSemanticWeb
{
    public partial class SemanticWeb
    {
        private const double ATTRACTION_CONSTANT = 0.1;		// spring constant
        private const double REPULSION_CONSTANT = 10000;	// charge constant

        private const double DEFAULT_DAMPING = 0.1;
        private const int DEFAULT_SPRING_LENGTH = 100;
        private const int DEFAULT_MAX_ITERATIONS = 500;

        public void Arrange()
        {
            Arrange(DEFAULT_DAMPING, DEFAULT_SPRING_LENGTH, DEFAULT_MAX_ITERATIONS, true);
        }

        /// <summary>
        /// Запуск алгоритма раскладки
        /// </summary>
        /// <param name="damping">Значение между 0 и 1, которое замедляет движение узлов</param>
        /// <param name="springLength">Значение в пикселях , представляющих длину мнимых источников, соединяющих узлы.</param>
        /// <param name="maxIterations">Максимальное число иттераций до конца</param>
        /// <param name="deterministic">Случайную или определенную раскладку использовать</param>
        public void Arrange(double damping, int springLength, int maxIterations, bool deterministic)
        {
            Random rnd = deterministic ? new Random(0) : new Random();

            // Копируем узлы в массив и задаем случайные координаты
            NodeLayoutInfo[] layout = new NodeLayoutInfo[this._nodes.Count];
            for (int i = 0; i < this._nodes.Count; i++)
            {
                layout[i] = new NodeLayoutInfo(this._nodes[i], new Vector(), Point.Empty);
                layout[i].Node.Position = new Point(rnd.Next(-50, 50), rnd.Next(-50, 50));
            }

            int stopCount = 0;
            int iterations = 0;

            while (true)
            {
                double totalDisplacement = 0;

                for (int i = 0; i < layout.Length; i++)
                {
                    NodeLayoutInfo current = layout[i];

                    // выражаем текущее положение узла в виде вектора , относительно начала
                    Vector currentPosition = new Vector(CalcDistance(Point.Empty, current.Node.Position), GetBearingAngle(Point.Empty, current.Node.Position));
                    Vector netForce = new Vector(0, 0);

                    // Определяем отталкивание
                    foreach (SemanticNode other in this._nodes)
                    {
                        if (other != current.Node) netForce += CalcRepulsionForce(current.Node, other);
                    }

                    // Опрделяем притяжение
                    foreach (SemanticNode  child in current.Node.OutArcs.Keys)
                    {
                        netForce += CalcAttractionForce(current.Node, child, springLength);
                    }
                    foreach (SemanticNode parent in this._nodes)
                    {
                        if (parent.OutArcs.Keys.Contains(current.Node)) netForce += CalcAttractionForce(current.Node, parent, springLength);
                    }
                    
                    current.Velocity = (current.Velocity + netForce) * damping;

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
                if (stopCount > 15) break;
                if (iterations > maxIterations) break;
            }

            // Центрируем сеть
            Rectangle logicalBounds = GetDiagramBounds();
            Point midPoint = new Point(logicalBounds.X + (logicalBounds.Width / 2), logicalBounds.Y + (logicalBounds.Height / 2));

            foreach (SemanticNode node in this._nodes)
            {
                node.Position -= (Size)midPoint;
            }
        }

        private Rectangle GetDiagramBounds()
        {
            int minX = Int32.MaxValue, minY = Int32.MaxValue;
            int maxX = Int32.MinValue, maxY = Int32.MinValue;
            foreach (SemanticNode node in this._nodes)
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
            double force = -(REPULSION_CONSTANT / Math.Pow(proximity, 2));
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
            double force = ATTRACTION_CONSTANT * Math.Max(proximity - springLength, 0);
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

        private class NodeLayoutInfo
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
