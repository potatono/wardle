using System;
using System.IO;
using System.Drawing;

namespace Wardle
{
    public partial class Map
    {
        private static Map? currentMap = null;

        public string? Name;
        public int Width;
        public int Height;
        public Dictionary<string, string> MetaData;
        public TerrainDesc[,] Terrain;

        public List<Unit> Units;

        public Map()
        {
            this.MetaData = new Dictionary<string, string>();
            this.Terrain = new TerrainDesc[0, 0];
            this.Units = new List<Unit>();
        }

        public static Map? Current
        {
            get { return currentMap; }
            set { currentMap = value; }
        }

        public static Map OpenAsCurrent(string path)
        {
            Map map = new Map();
            Map.Current = map.Open(path);
            return map;
        }

        public Map Open(string path)
        {
            try
            {
                this.MetaData = new Dictionary<string, string>();
                StreamReader reader = new StreamReader(path);
                bool readingMeta = true;
                string? line = reader.ReadLine();
                int y = 0;

                while (line != null)
                {
                    if (readingMeta)
                    {
                        readingMeta = parseMetaDataLine(line);
                    }
                    else
                    {
                        parseDataLine(y, line);
                        y++;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace?.ToString());
                System.Environment.Exit(1);
            }

            return this;
        }

        private bool parseMetaDataLine(string line)
        {
            Console.WriteLine(line);
            if (line == "")
            {
                this.Width = Int32.Parse(this.MetaData!["Width"]);
                this.Height = Int32.Parse(this.MetaData!["Height"]);
                this.Terrain = new TerrainDesc[this.Height, this.Width];
                return false;
            }
            else
            {
                string[] parts = line.Split(':', StringSplitOptions.TrimEntries);
                this.MetaData!.Add(parts[0], parts[1]);
                return true;
            }
        }

        private void parseDataLine(int y, string line)
        {
            for (int x = 0; x < this.Width; x++)
            {
                parseChar(line[x * 2], x, y);
                parseChar(line[x * 2 + 1], x, y);
            }
        }

        private void parseChar(char c, int x, int y)
        {
            if (MapDesc.Terrain.ContainsKey(c))
            {
                this.Terrain![y, x] = MapDesc.Terrain[c];
            }
            else if (MapDesc.Units.ContainsKey(c))
            {
                this.Units.Add(new Unit { Desc = MapDesc.Units[c], Position = new Point(x, y) });
            }
        }
    }
}