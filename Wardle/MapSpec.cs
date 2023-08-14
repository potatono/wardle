using System;

namespace Wardle
{

    public static class MapSpec
    {
        public class TerrainElement {
            public char Character;
            public string? Name;
            public ConsoleColor Color;
            public float Cost;
            public ConsoleColor BackgroundColor = ConsoleColor.Black;
        }

        public class UnitElement {
            public char Character;
            public string? Name;
            public bool isPlayer;
            public ConsoleColor Color;
            public float Speed;
        }

        public static Dictionary<char, TerrainElement> Terrain = new Dictionary<char, TerrainElement>() {
            { '=', new TerrainElement { Character='=', Name="Road", Color=ConsoleColor.Gray, Cost=1.0f } },
            { ':', new TerrainElement { Character=':', Name="Dirt", Color=ConsoleColor.DarkYellow, Cost=0.8f } },
            { ',', new TerrainElement { Character=',', Name="Grass", Color=ConsoleColor.DarkGreen, Cost=0.7f } },
            { '^', new TerrainElement { Character='^', Name="Hill", Color=ConsoleColor.DarkYellow, Cost=0.3f } },
            { '>', new TerrainElement { Character='>', Name="Mountain", Color=ConsoleColor.DarkRed, Cost=0 } },
            { '~', new TerrainElement { Character='~', Name="Water", Color=ConsoleColor.DarkBlue, Cost=0 } },
            { '#', new TerrainElement { Character='#', Name="Base", Color=ConsoleColor.Red, Cost=1.0f } }
        };

        public static Dictionary<char, UnitElement> Units = new Dictionary<char, UnitElement>() {
            { 'T', new UnitElement { Character='T', Name="PlayerTank", isPlayer=true, Color=ConsoleColor.Cyan, Speed=6 } },
            { 'I', new UnitElement { Character='I', Name="PlayerInfantry", isPlayer=true, Color=ConsoleColor.Cyan, Speed=2 } },
            { 'A', new UnitElement { Character='A', Name="PlayerArtillery", isPlayer=true, Color=ConsoleColor.Cyan, Speed=3 } },

            { 't', new UnitElement { Character='t', Name="EnemyTank", isPlayer=false, Color=ConsoleColor.Magenta, Speed=6 } },
            { 'i', new UnitElement { Character='i', Name="EnemyInfantry", isPlayer=false, Color=ConsoleColor.Magenta, Speed=2 } },
            { 'a', new UnitElement { Character='a', Name="EnemyArtillery", isPlayer=false, Color=ConsoleColor.Magenta, Speed=3 } }
        };
    }
}
