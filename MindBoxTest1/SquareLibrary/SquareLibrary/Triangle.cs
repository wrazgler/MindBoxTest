using System;

namespace SquareLibrary
{
    public class Triangle : IArea
    {
        private double _side1;
        private double _side2;
        private double _side3;
        private double _area; 

        public Triangle(double side1, double side2, double side3)
        {
            _side1 = side1;
            _side2 = side2;
            _side3 = side3;
        }

        public double Area()
        {
            if (!IsTriangle()) return 0;
            var p = (_side1 + _side2 + _side3) / 2;
            _area = Math.Sqrt(p * (p - _side1) * (p - _side2) * (p - _side3));
            return _area;
        }

        public bool IsTriangle()
        {
            if (_side1 >= (_side2 + _side3) || 
                _side2 >= (_side1 + _side3) || 
                _side3 >= (_side1 + _side2))
            {
                return false;
            }

            if (_side1 < 0 && _side2 < 0 && _side3 < 0) return false;

            return true;
        }

        public bool IsRightTriangle()
        {
            if ((_side1 == Math.Sqrt(Math.Pow(_side2, 2) + Math.Pow(_side3, 2))) ||
                (_side2 == Math.Sqrt(Math.Pow(_side1, 2) + Math.Pow(_side3, 2))) ||
                (_side3 == Math.Sqrt(Math.Pow(_side1, 2) + Math.Pow(_side2, 2))))
            {
                return true;
            }
            return false;
        }
    }
}
