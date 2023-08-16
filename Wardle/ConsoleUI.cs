using System.Drawing;

namespace Wardle {
    public class ConsoleLayer {
        public class Element {
            public ConsoleColor? BackgroundColor;
            public ConsoleColor? ForegroundColor;
            public char? Character;

            public bool Clear()
            {
                bool ret = (BackgroundColor is not null ||
                    ForegroundColor is not null ||
                    Character is not null);

                BackgroundColor = null;
                ForegroundColor = null;
                Character = null;

                return ret;
            }
        }


        public int Width;
        public int Height;
        public Element[,] Elements;

        public ConsoleLayer(int width, int height) {
            Elements = new Element[height, width];
            this.Width = width;
            this.Height = height;
            Init();
        }

        public void Init()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    Elements[y, x] = new Element();
        }

        public bool Clear()
        {
            bool ret = false;

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    ret = ret || Elements[y, x].Clear();

            return ret;
        }

        public bool Clear(int x, int y)
        {
            return Elements[y, x].Clear();
        }
    }

    public class ConsoleLayers {
        public int Width;
        public int Height;
        public ConsoleLayer[] Layers;

        public ConsoleLayers(int layers, int width, int height) {
            this.Width = width;
            this.Height = height;
            this.Layers = new ConsoleLayer[layers];
            for (int i=0; i<layers; i++) {
                this.Layers[i] = new ConsoleLayer(width, height);
            }
        }

        public char? Composite(int x, int y) {
            char? character = null;
            foreach (ConsoleLayer layer in Layers) {
                ConsoleLayer.Element e = layer.Elements[y,x];
                Console.ForegroundColor = e.ForegroundColor ?? Console.ForegroundColor;
                Console.BackgroundColor = e.BackgroundColor ?? Console.BackgroundColor;
                character = e.Character ?? character;
            }
            return character;
        }

        public char? Composite(Point p)
        {
            return Composite(p.X, p.Y);
        }

        public ConsoleLayer.Element Get(int l, int x, int y)
        {
            return Layers[l].Elements[y, x];
        }

        public ConsoleLayer.Element Get(int l, Point p)
        {
            return Get(l, p.X, p.Y);
        }

        public void Clear(int l)
        {
            Layers[l].Clear();
        }

        public void ClearWrite(int l)
        {
            for (int y=0; y<Height; y++)
            {
                for (int x=0; x<Width; x++)
                {
                    if (Layers[l].Clear(x, y))
                        Write(x, y);
                }
            }
        }

        public void ClearWrite(int l, int x, int y)
        {
            Layers[l].Clear(x, y);
            Write(x, y);
        }

        public void ClearWrite(int l, Point p)
        {
            ClearWrite(l, p.X, p.Y);
        }

        public void Write(int x, int y) {
            Console.SetCursorPosition(x, y);
            char? character = Composite(x, y);

            if (character != null)
                Console.Write(character);
        }

        public void Write(Point p)
        {
            Write(p.X, p.Y);
        }


        public void Write(int y) {
            Console.SetCursorPosition(0, y);

            for (int x=0; x<this.Width; x++) {
                Console.Write(Composite(x, y) ?? ' ');
            }
        }

        public void Write() {
            for (int y=0; y<Height; y++) {
                this.Write(y);
            }
        }
    }

    public class ConsoleUI {
        private enum Layer { Terrain=0, Units=1, Highlight=2, Interface=3 };
        private ConsoleLayers consoleLayers;
        private Map map;
        private int posX;
        private int posY;
        private Unit? lastSelected;

        public ConsoleUI(Map map) {
            this.consoleLayers = new ConsoleLayers(Enum.GetNames(typeof(Layer)).Length, map.Width * 2 + 2, map.Height);
            this.map = map;
            this.posX = map.Width / 2;
            this.posY = map.Height / 2;
            this.lastSelected = null;
        }

        public ConsoleUI() : this(Map.Current!) { }


        public Point Cursor
        {
            get { return new Point(posX, posY); }
        }

        public void WriteMapPoint(int x, int y) {
            TerrainDesc e = map.Terrain[y, x];

            ConsoleLayer.Element el = consoleLayers.Get((int)Layer.Terrain, x * 2 + 1, y);
            el.ForegroundColor = e.Color;
            el.BackgroundColor = e.BackgroundColor;
            el.Character = e.Character;

            el = consoleLayers.Get((int)Layer.Terrain, x * 2 + 2, y);
            el.ForegroundColor = e.Color;
            el.BackgroundColor = e.BackgroundColor;
            el.Character = e.Character;

            consoleLayers.Write(x * 2 + 1, y);
            consoleLayers.Write(x * 2 + 2, y);

            //Console.SetCursorPosition(x*2+1, y);
            //Console.ForegroundColor = e.Color;
            //Console.BackgroundColor = e.BackgroundColor;
            //Console.Write(e.Character);
            //Console.Write(e.Character);
            //Console.BackgroundColor = ConsoleColor.Black;
        }

        public void WriteMap() {
			Console.Clear();
			for (int y=0; y<map.Height; y++) {
				for (int x=0; x<map.Width; x++) {
                    WriteMapPoint(x, y);
                }
            }
        }


        public Point GetPointFromPosition(Point position)
        {
            return new Point(position.X * 2 + position.Y % 2 + 1, position.Y);
        }


        public void UnwriteUnit(Unit unit) 
        {
            consoleLayers.ClearWrite((int)Layer.Units, GetPointFromPosition(unit.Position));
        }

        public void WriteUnit(Unit unit)
        {
            Point p = GetPointFromPosition(unit.Position);
            ConsoleLayer.Element el = consoleLayers.Get((int)Layer.Units, p);
            el.ForegroundColor = unit.Desc.Color;
            el.Character = unit.Desc.Character;
            consoleLayers.Write(p);
        }

        public void WriteUnits() {
            int n = 10;

            foreach (Unit unit in map.Units) {
                WriteUnit(unit);

                Console.SetCursorPosition(65, n++);
                Console.Write(n + ":" + unit.Desc.Character + " " + unit.Position.X + "," + unit.Position.Y);
                //Console.SetCursorPosition(x, y);
                //Console.ForegroundColor = unit.Element.Color;
                //Console.Write(unit.Element.Character);
            }
        }

        public TerrainDesc GetMapAtCursorPosition(int x, int y) {
            return map.Terrain[y, x/2];
        }

        public void WriteCursor() {
            int x = posX * 2 + (posY % 2);
            int y = posY;

            ConsoleLayer.Element el = consoleLayers.Get((int)Layer.Interface, x, y);
            el.ForegroundColor = ConsoleColor.Yellow;
            el.Character = '[';
            consoleLayers.Write(x, y);

            x += 2;
            el = consoleLayers.Get((int)Layer.Interface, x, y);
            el.ForegroundColor = ConsoleColor.Yellow;
            el.Character = ']';
            consoleLayers.Write(x, y);

            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.SetCursorPosition(posX*2+(posY%2), posY);
            //Console.Write('[');
            //Console.SetCursorPosition(posX*2+(posY%2)+2, posY);
            //Console.Write(']');
            //Console.SetCursorPosition(0, 0);

            Console.SetCursorPosition(65, 0);
            Console.Write(posX + "," + posY);
        }

        public void Unwrite(int x, int y) {
            ConsoleLayer.Element el = consoleLayers.Get((int)Layer.Interface, x, y);
            el.ForegroundColor = null;
            el.Character = null;
            consoleLayers.Write(x, y);



            //if (x == 0) {
            //    Console.SetCursorPosition(0, y);
            //    Console.Write(' ');
            //}
            //else if (x > map.Width * 2) {
            //    Console.SetCursorPosition(map.Width * 2 + 1, y);
            //    Console.Write(' ');
            //}
            //else {
            //    MapSpec.TerrainElement el = GetMapAtCursorPosition(x - (1 - y%2), y);
            //    Console.SetCursorPosition(x, y);
            //    Console.ForegroundColor = el.Color;
            //    Console.Write(el.Character);
            //}
        }

        public void ClearCursor() {
            Unwrite(posX*2+(posY%2), posY);
            Unwrite(posX*2+(posY%2)+2, posY);
            Console.SetCursorPosition(0, 0);
        }

        public Unit? GetSelectedUnit() {
            foreach (Unit unit in map.Units) {
                if (unit.Position.X == posX && unit.Position.Y == posY)
                    return unit;
            }

            return null;
        }
        
        public void Select() {
            Unit? unit = GetSelectedUnit();

            consoleLayers.ClearWrite((int)Layer.Highlight);

            if (unit != null)
            {
                lastSelected = unit;

                Point[] validMoves = unit.GetValidMoves();

                foreach (Point p in validMoves)
                {
                    consoleLayers.Get((int)Layer.Highlight, p.X * 2, p.Y).BackgroundColor = ConsoleColor.DarkGray;
                    consoleLayers.Write(p.X * 2, p.Y);

                    consoleLayers.Get((int)Layer.Highlight, p.X * 2 + 1, p.Y).BackgroundColor = ConsoleColor.DarkGray;
                    consoleLayers.Write(p.X * 2 + 1, p.Y);
                }
            }
            else if (lastSelected != null && lastSelected.isValidMove(Cursor))
            {
                UnwriteUnit(lastSelected);

                lastSelected.Move(Cursor);

                WriteUnit(lastSelected);

                lastSelected = null;
            }
        }

        public void Run() {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            WriteMap();
            WriteUnits();

            ConsoleKeyInfo cki;

            do {
                WriteCursor();

                cki = Console.ReadKey(false);
                int ofsX = 0;
                int ofsY = 0;

                if (cki.Key == ConsoleKey.UpArrow) ofsY = -1;
                else if (cki.Key == ConsoleKey.DownArrow) ofsY = 1;
                else if (cki.Key == ConsoleKey.LeftArrow) ofsX = -1;
                else if (cki.Key == ConsoleKey.RightArrow) ofsX = 1;
                else if (cki.Key == ConsoleKey.Enter) Select();

                if (ofsX != 0 || ofsY != 0) {
                    ClearCursor();

                    posX += ofsX;
                    posY += ofsY;

                    if (posX < 0) posX = 0;
                    else if (posX >= map.Width) posX = map.Width-1;

                    if (posY < 0) posY = 0;
                    else if (posY >= map.Height) posY = map.Height-1;
                }

            }
            while (cki.Key != ConsoleKey.Escape);
            //Console.SetCursorPosition(0, map.Height);
            Console.Clear();
        }
    }
}
