using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
        private int Width, Height;

        public void Arrange(int width, int height)
        {
            Width = width;
            Height = height;
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
            DrawThread thread = new DrawThread(Width, Height, ATTRACTION_CONSTANT, REPULSION_CONSTANT, damping,
                                               springLength, maxIterations, this, deterministic);
            thread.Run();
        }

       
    }
}
