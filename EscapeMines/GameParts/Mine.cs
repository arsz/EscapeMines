using System;
using System.Collections.Generic;

namespace EscapeMines.GameParts
{
	public class Mine : Tile
	{
		public override string GetStatus() => "Mine Hit";

		public Mine(int x,int y) :base(x,y)
		{

		}
	}
}
