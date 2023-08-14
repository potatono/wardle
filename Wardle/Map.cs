using System;
using System.IO;
using System.Drawing;

namespace Wardle
{
    public class Map
    {
        public class Unit {
            public required MapSpec.UnitElement Element;
            public Point Position;
        }

        public string? Name;
        public int Width;
        public int Height;
        public Dictionary<string, string> MetaData;
        public MapSpec.TerrainElement[,] Terrain;

        public List<Unit> Units;

        public Map() {
            this.MetaData = new Dictionary<string, string>();
            this.Terrain = new MapSpec.TerrainElement[0,0];
            this.Units = new List<Unit>();
        }

        public void Open(string path) {
            try {
                this.MetaData = new Dictionary<string, string>();
                StreamReader reader = new StreamReader(path);
                bool readingMeta = true;
                string? line = reader.ReadLine();
                int y = 0;

                while (line != null) {
                    if (readingMeta) {
                        readingMeta = parseMetaDataLine(line);
                    }
                    else {
                        parseDataLine(y, line);
                        y++;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception exc) {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace?.ToString());
                System.Environment.Exit(1);
            }
        }

        private bool parseMetaDataLine(string line) {
            Console.WriteLine(line);
            if (line == "") {
                this.Width = Int32.Parse(this.MetaData!["Width"]);
                this.Height = Int32.Parse(this.MetaData!["Height"]);
                this.Terrain = new MapSpec.TerrainElement[this.Height, this.Width];
                return false;
            }
            else {
                string[] parts = line.Split(':', StringSplitOptions.TrimEntries);
                this.MetaData!.Add(parts[0], parts[1]);
                return true;
            }
        }

        private void parseDataLine(int y, string line) {
            for (int x=0; x<this.Width; x++) {
                parseChar(line[x*2], x, y);
                parseChar(line[x*2+1], x, y);
            }
        }

        private void parseChar(char c, int x, int y) {
            if (MapSpec.Terrain.ContainsKey(c)) { 
                this.Terrain![y,x] = MapSpec.Terrain[c];
            }
            else if (MapSpec.Units.ContainsKey(c)) {
                this.Units.Add(new Unit { Element=MapSpec.Units[c], Position=new Point(x, y) });
            }
        }
    }
}