using System;

namespace Wardle
{

    public static partial class MapDesc
    {

        public static Dictionary<char, TerrainDesc> Terrain = new Dictionary<char, TerrainDesc>() {
            { '=', new TerrainDesc { Character='=', Name="Road", Color=ConsoleColor.Gray, Cost=1.0f } },
            { ':', new TerrainDesc { Character=':', Name="Dirt", Color=ConsoleColor.DarkYellow, Cost=0.8f } },
            { ',', new TerrainDesc { Character=',', Name="Grass", Color=ConsoleColor.DarkGreen, Cost=0.7f } },
            { '^', new TerrainDesc { Character='^', Name="Hill", Color=ConsoleColor.DarkYellow, Cost=0.3f } },
            { '>', new TerrainDesc { Character='>', Name="Mountain", Color=ConsoleColor.DarkRed, Cost=0 } },
            { '~', new TerrainDesc { Character='~', Name="Water", Color=ConsoleColor.DarkBlue, Cost=0 } },
            { '#', new TerrainDesc { Character='#', Name="Base", Color=ConsoleColor.Red, Cost=1.0f } }
        };

        public static Dictionary<char, UnitDesc> Units = new Dictionary<char, UnitDesc>() {
            { 'T', new UnitDesc { Character='T', Name="PlayerTank", isPlayer=true, Color=ConsoleColor.Cyan, Speed=6 } },
            { 'I', new UnitDesc { Character='I', Name="PlayerInfantry", isPlayer=true, Color=ConsoleColor.Cyan, Speed=2 } },
            { 'A', new UnitDesc { Character='A', Name="PlayerArtillery", isPlayer=true, Color=ConsoleColor.Cyan, Speed=3 } },

            { 't', new UnitDesc { Character='t', Name="EnemyTank", isPlayer=false, Color=ConsoleColor.Magenta, Speed=6 } },
            { 'i', new UnitDesc { Character='i', Name="EnemyInfantry", isPlayer=false, Color=ConsoleColor.Magenta, Speed=2 } },
            { 'a', new UnitDesc { Character='a', Name="EnemyArtillery", isPlayer=false, Color=ConsoleColor.Magenta, Speed=3 } }
        };
    }
}
