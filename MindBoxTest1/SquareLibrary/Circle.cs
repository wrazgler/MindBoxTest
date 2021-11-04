using System;

namespace SquareLibrary
{
    public class Circle : IArea
    {
        private double _radius;
        private double _area;

        public Circle(double radius)
        {
            _radius = radius;
        }

        public double Area()
        {
            _area = Math.Round((Math.PI * Math.Pow(_radius, 2)), 2);
            return _area;
        }
    }
}
