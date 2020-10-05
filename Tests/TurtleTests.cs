using EscapeMines.GameParts;
using NUnit.Framework;
using System;

namespace Tests
{
	public class TurtleTests
	{
		[Test]
		public void Rotate_ShouldThrowInvalidOperationExceptionIfMovementIsNotARotation()
		{
			//Arrange
			var tile = new Tile(0, 0);
			var startingDirection = Direction.NorthDirection;
			var turtle = new Turtle(tile, startingDirection);

			//Act - Assert
			Assert.Throws<InvalidOperationException>(()=> turtle.Rotate(Movement.Move));
		}

		[Test]
		public void Rotate_ShouldChangeDirectionForSouthIfTheOriginalDirectionWasWestInCaseOfLeftRotation()
		{
			//Arrange
			var tile = new Tile(0, 0);
			var startingDirection = Direction.WestDirection;
			var turtle = new Turtle(tile, startingDirection);

			//Act - Assert
			turtle.Rotate(Movement.LeftRotate);

			//Assert
			Assert.That(turtle.Direction == Direction.SouthDirection);
		}

		[Test]
		public void Rotate_ShouldChangeDirectionForWestIfTheOriginalDirectionWasNorthInCaseOfLeftRotation()
		{
			//Arrange
			var tile = new Tile(0, 0);
			var startingDirection = Direction.NorthDirection;
			var turtle = new Turtle(tile, startingDirection);

			//Act - Assert
			turtle.Rotate(Movement.LeftRotate);

			//Assert
			Assert.That(turtle.Direction == Direction.WestDirection);
		}

		[Test]
		public void Rotate_ShouldChangeDirectionForNorthIfTheOriginalDirectionWasEastInCaseOfLeftRotation()
		{
			//Arrange
			var tile = new Tile(0, 0);
			var startingDirection = Direction.EastDirection;
			var turtle = new Turtle(tile, startingDirection);

			//Act - Assert
			turtle.Rotate(Movement.LeftRotate);

			//Assert
			Assert.That(turtle.Direction == Direction.NorthDirection);
		}

		[Test]
		public void Rotate_ShouldChangeDirectionForEastIfTheOriginalDirectionWasSouthInCaseOfLeftRotation()
		{
			//Arrange
			var tile = new Tile(0, 0);
			var startingDirection = Direction.SouthDirection;
			var turtle = new Turtle(tile, startingDirection);

			//Act - Assert
			turtle.Rotate(Movement.LeftRotate);

			//Assert
			Assert.That(turtle.Direction == Direction.EastDirection);
		}

		[Test]
		public void Rotate_ShouldChangeDirectionForWestIfTheOriginalDirectionWasSouthInCaseOfRightRotation()
		{
			//Arrange
			var tile = new Tile(0, 0);
			var startingDirection = Direction.SouthDirection;
			var turtle = new Turtle(tile, startingDirection);

			//Act - Assert
			turtle.Rotate(Movement.RightRotate);

			//Assert
			Assert.That(turtle.Direction == Direction.WestDirection);
		}

		[Test]
		public void Rotate_ShouldChangeDirectionForNorthIfTheOriginalDirectionWasWestInCaseOfRightRotation()
		{
			//Arrange
			var tile = new Tile(0, 0);
			var startingDirection = Direction.WestDirection;
			var turtle = new Turtle(tile, startingDirection);

			//Act - Assert
			turtle.Rotate(Movement.RightRotate);

			//Assert
			Assert.That(turtle.Direction == Direction.NorthDirection);
		}

		[Test]
		public void Rotate_ShouldChangeDirectionForEastIfTheOriginalDirectionWasNorthInCaseOfRightRotation()
		{
			//Arrange
			var tile = new Tile(0, 0);
			var startingDirection = Direction.NorthDirection;
			var turtle = new Turtle(tile, startingDirection);

			//Act - Assert
			turtle.Rotate(Movement.RightRotate);

			//Assert
			Assert.That(turtle.Direction == Direction.EastDirection);
		}

		[Test]
		public void Rotate_ShouldChangeDirectionForSouthIfTheOriginalDirectionWasEastInCaseOfRightRotation()
		{
			//Arrange
			var tile = new Tile(0, 0);
			var startingDirection = Direction.EastDirection;
			var turtle = new Turtle(tile, startingDirection);

			//Act - Assert
			turtle.Rotate(Movement.RightRotate);

			//Assert
			Assert.That(turtle.Direction == Direction.SouthDirection);
		}

		[TestCase(Movement.RightRotate)]
		[TestCase(Movement.LeftRotate)]
		public void Move_ShouldThrowInvalidOperationExceptionIfTheMovementIsNotMove(Movement movement)
		{
			//Arrange
			var board = new Tile[3,3];
			var oldTile = new Tile(1,1);
			var startingDirection = Direction.NorthDirection;
			var turtle = new Turtle(oldTile, startingDirection);
		

			//Act - Assert
			Assert.Throws<InvalidOperationException>(() => turtle.Move(movement, board));
		}

		[Test]
		public void Move_ShouldChangeTurtleTileTo01From11AfterNorthMoving()
		{
			//Arrange
			var board = new Tile[3, 3];
			var oldTile = new Tile(1, 1);
			var newTile = new Tile(0, 1);
			board[newTile.X, newTile.Y] = newTile;
			var startingDirection = Direction.NorthDirection;
			var turtle = new Turtle(oldTile, startingDirection);

			//Act
			turtle.Move(Movement.Move, board);

			//Assert
			Assert.That(turtle.Tile == newTile);
		}

		[Test]
		public void Move_ShouldChangeTurtleTileTo21From11AfterSouthMoving()
		{
			//Arrange
			var board = new Tile[3, 3];
			var oldTile = new Tile(1, 1);
			var newTile = new Tile(2, 1);
			board[newTile.X, newTile.Y] = newTile;
			var startingDirection = Direction.SouthDirection;
			var turtle = new Turtle(oldTile, startingDirection);

			//Act
			turtle.Move(Movement.Move, board);

			//Assert
			Assert.That(turtle.Tile == newTile);
		}

		[Test]
		public void Move_ShouldChangeTurtleTileTo12From11AfterEastMoving()
		{
			//Arrange
			var board = new Tile[3, 3];
			var oldTile = new Tile(1, 1);
			var newTile = new Tile(1, 2);
			board[newTile.X, newTile.Y] = newTile;
			var startingDirection = Direction.EastDirection;
			var turtle = new Turtle(oldTile, startingDirection);

			//Act
			turtle.Move(Movement.Move, board);

			//Assert
			Assert.That(turtle.Tile == newTile);
		}

		[Test]
		public void Move_ShouldChangeTurtleTileTo10From11AfterWestMoving()
		{
			//Arrange
			var board = new Tile[3, 3];
			var oldTile = new Tile(1, 1);
			var newTile = new Tile(1,0);
			board[newTile.X, newTile.Y] = newTile;
			var startingDirection = Direction.WestDirection;
			var turtle = new Turtle(oldTile, startingDirection);

			//Act
			turtle.Move(Movement.Move, board);

			//Assert
			Assert.That(turtle.Tile == newTile);
		}
	}
}
