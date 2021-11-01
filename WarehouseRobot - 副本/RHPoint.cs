using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseRobot
{
    public struct RHPoint : IEquatable<RHPoint>
    {
        public static readonly RHPoint Empty = new(0,0);
        public int X { readonly get; set; }
        public int Y { readonly get; set; }

        public string Description { get; set; }
        public RHPoint(int x, int y, string description = "")
        {
            X = x;
            Y = y;
            Description = description;
        }

        public RHPoint(Point pt)
        {
            X = pt.X;
            Y = pt.Y;
            Description = string.Empty;
        }
        public readonly bool IsEmpty
        {
            get
            {
                return X == 0 && Y == 0;
            }
        }

        public bool Equals(RHPoint other)
        {
            return X == other.X && Y == other.Y;
        }

        public static RHPoint operator +(RHPoint pt, Size sz)
        {
            return new RHPoint(pt.X + sz.Width, pt.Y + sz.Width);
        }

        public static RHPoint operator -(RHPoint pt, Size sz)
        {
            return new RHPoint(pt.X - sz.Width, pt.Y - sz.Width);
        }
     
        public static bool operator ==(RHPoint left, RHPoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RHPoint left, RHPoint right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            return obj is RHPoint point && Equals(point);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }
    }
}
