using System;
using System.Drawing;

namespace Wardle
{
	public class Unit
	{
		public required UnitDesc Desc;
		public Point Position;

		private Point[]? validMoves;

		public Point[] GetValidMoves()
		{
			if (validMoves is null)
			{
				PathFinder pf = new PathFinder(this);
				pf.Find();
				validMoves = pf.Points.ToArray();
			}

			return validMoves;
		}

		public bool isValidMove(Point p)
		{
			return GetValidMoves().Contains(p);
		}

		public bool Move(Point p)
		{
			validMoves = GetValidMoves();
			if (validMoves.Contains(p))
			{
				Position = p;
				validMoves = null;

				return true;
			}

			return false;
		}
	}
}

