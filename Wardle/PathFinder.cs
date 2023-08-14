using System.Drawing;

namespace Wardle {
    public class PathFinder {
        
        private Map map;
        private Map.Unit unit;
        private Dictionary<int, bool> added;
        public List<Point> Points;
        public int LookCount;

        public PathFinder(Map map, Map.Unit unit) {
            this.map = map;
            this.unit = unit;
            this.added = new Dictionary<int, bool>();
            this.Points = new List<Point>();
            this.LookCount = 0;
        }

        public void Find() {
            Find(unit.Position.X, unit.Position.Y, unit.Element.Speed);
        }

        public void Find(int x, int y, float moves) {
            int ofs = y % 2 == 0 ? 1 : -1;

            // Up Left / Up Right
            Look(x + ofs, y - 1, moves);
            Look(x, y - 1, moves);

            // Left / Right
            Look(x - 1, y, moves);
            Look(x + 1, y, moves);

            // Down Left / Down Right
            Look(x + ofs, y + 1, moves);
            Look(x, y + 1, moves);
        }

        public void Look(int x, int y, float moves) {
            int key = y * map.Width + x;
            this.LookCount++;

            if (x < 0 || y < 0 || x >= map.Width || y >= map.Height)
                return;

            float cost = map.Terrain[y,x].Cost;

            if (cost == 0)
                return;

            if (moves < 1.0f / cost)
                return;

            Find(x, y, moves - 1.0f / cost);
            if (!added.ContainsKey(key)) {
                Points.Add(new Point(x, y));
                added[key] = true;
            }
        }
    }
}