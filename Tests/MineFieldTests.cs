using EscapeMines.GameParts;
using EscapeMines.GameParts.Interfaces;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
	public class MineFieldTests
	{
		Mock<IMovementFactory> mockedMovementFactory;
		Minefield minefield;

		[SetUp]
		public void Setup()
		{
			mockedMovementFactory = new Mock<IMovementFactory>();
		}

		[Test]
		public void GetResult_ShouldReturnTurtleCurrentTileStatus()
		{
			//Arrange
			var testDirection = Direction.NorthDirection;
			var testTile = new Tile(0, 1);
			var testTurtle = new Turtle(testTile, testDirection);
			var tiles = new Tile[2, 2];
			tiles[testTile.X, testTile.Y] = testTile;
			minefield = new Minefield(mockedMovementFactory.Object, tiles, new Mine[0], new ExitPoint(0, 0), testTurtle);

			//Act
			var result = minefield.GetResult();

			//Assert
			Assert.That(result == testTurtle.Tile.GetStatus());
		}

		[Test]
		public void FillUpBoard_ShouldFillUpMinesToBoardAndTakeTheExitPoint()
		{
			//Arrange
			var testDirection = Direction.NorthDirection;
			var testTile = new Tile(0, 1);
			var testTurtle = new Turtle(testTile, testDirection);
			var tiles = new Tile[2, 2];
			tiles[0, 1] = testTile;
			var mines = new Mine[] { new Mine(1, 1), new Mine(1,0)};

			minefield = new Minefield(mockedMovementFactory.Object, tiles, mines, new ExitPoint(0, 0), testTurtle);

			//Act 
			minefield.FillUpBoard();

			//Assert
			Assert.That(tiles[1, 1] is Mine);
			Assert.That(tiles[1, 0] is Mine);
			Assert.That(tiles[0, 1] is Mine == false);
			Assert.That(tiles[0, 0] is ExitPoint);
		}

		[Test]
		public void FillUpBoard_ShouldFillUpAllRemainingFieldOnTheBoardAfterSpecifiedFieldsHaveTaken()
		{
			//Arrange
			var testDirection = Direction.NorthDirection;
			var testTile = new Tile(0, 1);
			var testTurtle = new Turtle(testTile, testDirection);
			var tiles = new Tile[3, 2];
			var mines = new Mine[] { new Mine(1, 1), new Mine(1, 0) };

			minefield = new Minefield(mockedMovementFactory.Object, tiles, mines, new ExitPoint(0, 0), testTurtle);

			//Act 
			minefield.FillUpBoard();

			//Assert
			Assert.That(tiles[2, 0] != null);
			Assert.That(tiles[2, 1] != null);
			Assert.That(tiles[1, 1] != null);
			Assert.That(tiles[1, 0] != null);
			Assert.That(tiles[0, 1] != null);
			Assert.That(tiles[0, 0] != null);
		
		}

		[Test]
		public async Task IsStillMovingOnAsync_ShouldReturnTrueAfterMineOrExitPointDoNotFound()
		{
			//Arrange
			var testTile = new Tile(0, 1); 
			var tiles = new Tile[3, 2];
			var mines = new Mine[] { new Mine(1, 1), new Mine(1, 0) };

			var mockedTurtle = new Mock<IDancer>();
			mockedTurtle.SetupGet(x => x.Tile).Returns(testTile);

			var movements = new Movement[] { Movement.Move, Movement.LeftRotate, Movement.Move, Movement.RightRotate, Movement.Move };
			mockedMovementFactory.Setup(x => x.GetNextMovementsAsync()).Returns(Task.FromResult(movements));

			minefield = new Minefield(mockedMovementFactory.Object, tiles, mines, new ExitPoint(0, 0), mockedTurtle.Object);

			//Act
			var isFinished = await minefield.IsStillMovingOnAsync();

			//Assert
			Assert.IsTrue(isFinished);
		}

		[Test]
		public async Task IsStillMovingOnAsync_ShouldReturnFalseAfterHitMine()
		{
			//Arrange
			var tiles = new Tile[3, 2];
			var mines = new Mine[] { new Mine(1, 1) };

			var mockedTurtle = new Mock<IDancer>();
			mockedTurtle.SetupGet(x => x.Tile).Returns(mines.First());

			var movements = new Movement[] { Movement.Move, Movement.LeftRotate, Movement.Move, Movement.RightRotate, Movement.Move };
			mockedMovementFactory.Setup(x => x.GetNextMovementsAsync()).Returns(Task.FromResult(movements));

			minefield = new Minefield(mockedMovementFactory.Object, tiles, mines, new ExitPoint(0, 0), mockedTurtle.Object);

			//Act
			var isFinished = await minefield.IsStillMovingOnAsync();

			//Assert
			Assert.IsFalse(isFinished);
		}

		[Test]
		public async Task IsStillMovingOnAsync_ShouldReturnFalseAfterReachedTheExitPoint()
		{
			//Arrange
			var tiles = new Tile[3, 2];
			var mines = new Mine[] { new Mine(1, 1) };
			var exitPoint = new ExitPoint(0, 0);
			var mockedTurtle = new Mock<IDancer>();
			mockedTurtle.SetupGet(x => x.Tile).Returns(exitPoint);

			var movements = new Movement[] { Movement.Move, Movement.LeftRotate, Movement.Move, Movement.RightRotate, Movement.Move };
			mockedMovementFactory.Setup(x => x.GetNextMovementsAsync()).Returns(Task.FromResult(movements));

			minefield = new Minefield(mockedMovementFactory.Object, tiles, mines, exitPoint, mockedTurtle.Object);

			//Act
			var isFinished = await minefield.IsStillMovingOnAsync();

			//Assert
			Assert.IsFalse(isFinished);
		}

		[Test]
		public async Task IsStillMovingOnAsync_ShouldReturnTrueIfDancerDoesNotMoveAnywhereDueToMines()
		{
			//Arrange
			var tiles = new Tile[3, 2];
			var mines = new Mine[] { new Mine(1, 1) };
			var exitPoint = new ExitPoint(0, 0);
			var mockedTurtle = new Mock<IDancer>();
			mockedTurtle.SetupGet(x => x.Tile).Returns(mines.First());

			var movements = new Movement[] { Movement.LeftRotate, Movement.RightRotate };
			mockedMovementFactory.Setup(x => x.GetNextMovementsAsync()).Returns(Task.FromResult(movements));

			minefield = new Minefield(mockedMovementFactory.Object, tiles, mines, exitPoint, mockedTurtle.Object);

			//Act
			var isFinished = await minefield.IsStillMovingOnAsync();

			//Assert
			Assert.IsTrue(isFinished);
		}

		[Test]
		public async Task IsStillMovingOnAsync_ShouldReturnFalseIfMovementsIsEmpty()
		{
			//Arrange
			var testTile = new Tile(0, 1);
			var tiles = new Tile[3, 2];
			var mines = new Mine[] { new Mine(1, 1) };
			var exitPoint = new ExitPoint(0, 0);
			var mockedTurtle = new Mock<IDancer>();
			mockedTurtle.SetupGet(x => x.Tile).Returns(testTile);

			var movements = new Movement[] {};
			mockedMovementFactory.Setup(x => x.GetNextMovementsAsync()).Returns(Task.FromResult(movements));

			minefield = new Minefield(mockedMovementFactory.Object, tiles, mines, exitPoint, mockedTurtle.Object);

			//Act
			var isFinished = await minefield.IsStillMovingOnAsync();

			//Assert
			Assert.IsFalse(isFinished);
		}

	}
}
