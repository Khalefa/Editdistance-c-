using System;
namespace RTree
{
    public class pair
    {
        public pair(object f, object s)
        {
            first = f; second = s;
        }
        public object first, second;
    }
    public class Point
    {
        
        public Point parent;
        public int level;
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < DIMENSIONS; i++) s = s + coordinates[i] + ",";
            return s.TrimEnd(',');
        }
        int DIMENSIONS = 2;
        internal int[] coordinates;
        public Point(Point p)
        {
            DIMENSIONS = p.DIMENSIONS;
            coordinates = new int[DIMENSIONS];
            for (int i = 0; i < DIMENSIONS; i++)
                coordinates[i] = p.coordinates[i];
        }
        public Point(float x, float y, int dim = 2)
        {
            DIMENSIONS = dim;
            coordinates = new int[DIMENSIONS];
            coordinates[0] = (int)x;
            coordinates[1] = (int)y;
            //coordinates[2] = z;
        }
        public Point(int[] corrd, int dim)
        {
            DIMENSIONS = dim;
            coordinates = new int[DIMENSIONS];
            for (int i = 0; i < dim; i++)
            {
                coordinates[i] = corrd[i];
            }
        }
        public override int GetHashCode()
        {
            int s = DIMENSIONS;
            for (int i = 0; i < DIMENSIONS; i++) s = s + coordinates[i];
            if (parent != null)
                s = s + parent.GetHashCode();
            return s;
        }
        public override bool Equals(object obj)
        {
            Point o = (Point)obj;

            if ((parent != null) && (o.parent != null))
            {
                if (parent.GetHashCode() != o.parent.GetHashCode()) return false;
            }

            if (DIMENSIONS != o.DIMENSIONS) return false;
            for (int i = 0; i < DIMENSIONS; i++)
            {
                if (o.coordinates[i] != coordinates[i]) return false;
            }
            return true;
        }
    }
}