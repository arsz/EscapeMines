using System;

namespace EscapeMines.GameParts
{
	public class ExitPoint : Tile
	{
		public override string GetStatus() => "Success";

		public ExitPoint(int x, int y) : base(x, y)
		{
		}

	}
}
