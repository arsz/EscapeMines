using System;

namespace EscapeMines.GameParts
{
	public class Tile
	{
		public int X { get; }

		public int Y { get; }

		public Tile(int x, int y)
		{
			X = x;
			Y = y;
		}

		public virtual string GetStatus() => "Still in Danger";

	}
}
