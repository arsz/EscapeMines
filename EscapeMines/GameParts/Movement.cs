using System;
using System.Collections.Generic;
using System.Linq;

namespace EscapeMines.GameParts
{
	public enum Movement
	{
		Move = 'M',
		RightRotate = 'R',
		LeftRotate = 'L'
	}

	public static class MovementRules
	{
		private readonly static Direction[] DirectionInOrder = new Direction[]
		{
				Direction.EastDirection,
				Direction.SouthDirection,
				Direction.WestDirection,
				Direction.NorthDirection
		};

		public static (int X, int Y) Move(this Movement movement, Direction currentDirection)
		{
			if (movement != Movement.Move) throw new InvalidOperationException("Cannot move with roteting operation");

			switch (currentDirection)
			{
				case Direction.WestDirection:
					return (0, -1);
				case Direction.EastDirection:
					return (0, 1);
				case Direction.NorthDirection:
					return (-1, 0);
				case Direction.SouthDirection:
					return (1, 0);
				default:
					throw new ArgumentOutOfRangeException($"Invalid direction: {currentDirection}!");
			}
		}

		public static Direction Rotate(this Movement movement, Direction oldDirection)
		{
			if (movement == Movement.Move) throw new InvalidOperationException("Cannot rotate with moving operation");

			return movement.GetNewDirection(oldDirection);
		}


		private static Direction GetNewDirection(this Movement movement, Direction originalDiraction)
		{

			var index = Array.IndexOf(DirectionInOrder, originalDiraction);
			var nextDirectionOperation = movement == Movement.LeftRotate ? -1 : 1;
			index += nextDirectionOperation;

			if (index == DirectionInOrder.Length)
			{
				return DirectionInOrder.First();
			}
			else if (index == -1)
			{
				return DirectionInOrder.Last();
			}

			return DirectionInOrder[index];
		}
	}
}
