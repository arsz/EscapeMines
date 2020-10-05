using EscapeMines.GameParts.Interfaces;
using System;

namespace EscapeMines.GameParts
{
	public class Turtle : IDancer
	{

		public Turtle(Tile startingTile, Direction startingDirection)
		{
			Direction = startingDirection;
			Tile = startingTile;
		}

		public Direction Direction { get; private set; }

		public Tile Tile { get; private set; }

		public void Rotate(Movement movement)
		{
			var newDirection = movement.Rotate(Direction);
			Direction = newDirection;
		}

		public void Move(Movement movement, Tile[,] board)
		{
			var newMovingDirectionDifferencial = movement.Move(Direction);
			var newX = Tile.X + newMovingDirectionDifferencial.X;
			var newY = Tile.Y + newMovingDirectionDifferencial.Y;
			Tile = board[newX, newY];
		}
	}
}
