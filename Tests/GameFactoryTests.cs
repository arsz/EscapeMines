using EscapeMines.GameParts;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
	public class GameFactoryTests : GameFactory
	{

		[Test]
		public async Task InitBoardAsync_ShouldInvokeFourthTimesTheGetNextLineContent_IfIthasNext()
		{
			//Arrange
			var mockedInputAsyncEnumerator = new Mock<IAsyncEnumerator<string>>();
			mockedInputAsyncEnumerator.Setup(x => x.MoveNextAsync()).Returns(new ValueTask<bool>(true));

			//Act
			await InitBoardAsync(mockedInputAsyncEnumerator.Object);

			//Assert
			mockedInputAsyncEnumerator.Verify(x => x.MoveNextAsync(), Times.Exactly(4));
		}

		[Test]
		public void InitBoardAsync_ShouldThrowArgumentException_IfIthasnNext()
		{
			//Arrange
			var mockedInputAsyncEnumerator = new Mock<IAsyncEnumerator<string>>();
			mockedInputAsyncEnumerator.Setup(x => x.MoveNextAsync()).Returns(new ValueTask<bool>(false));

			//Act - Assert
			Assert.ThrowsAsync<ArgumentException>(() => InitBoardAsync(mockedInputAsyncEnumerator.Object));
		}

		[Test]
		public async Task GetNextMovementsAsync_ShouldReturnEmptyArrayIfNextLineIsEmpty()
		{
			//Arrange
			var mockedInputAsyncEnumerator = new Mock<IAsyncEnumerator<string>>();
			mockedInputAsyncEnumerator.Setup(x => x.MoveNextAsync()).Returns(new ValueTask<bool>(false));

			asyncInputEnumerator = mockedInputAsyncEnumerator.Object;

			//Act
			var array = await GetNextMovementsAsync();

			Assert.IsEmpty(array);
		}

		[Test]
		public async Task GetNextMovementsAsync_ShouldReturnMovementsArrayWithRelatedMovementsInCaseOfCorrectInput()
		{
			//Arrange
			var mockedInputAsyncEnumerator = new Mock<IAsyncEnumerator<string>>();
			mockedInputAsyncEnumerator.Setup(x => x.MoveNextAsync()).Returns(new ValueTask<bool>(true));
			mockedInputAsyncEnumerator.Setup(x => x.Current).Returns("R M L M M L");
			asyncInputEnumerator = mockedInputAsyncEnumerator.Object;

			//Act
			var array = await GetNextMovementsAsync();

			Assert.IsNotEmpty(array);
			Assert.That(array.Count(x => x == Movement.Move) == 3);
			Assert.That(array.Count(x => x == Movement.RightRotate) == 1);
			Assert.That(array.Count(x => x == Movement.LeftRotate) == 2);
		}

		[Test]
		public void WithMines_ShouldThrowExceptionInCaseOfInvalidInput()
		{
			inputMinesCoordinates = "1,1 1,3,3";

			Assert.Throws<ArgumentOutOfRangeException>(() => WithMines());
		}

		[Test]
		public void WithMines_ShouldReturnThisInCaseOfCorrectInputAndContainsAllCoordinatesAreUnique()
		{
			this.inputMinesCoordinates = "1,1 1,3 3,3";

			//Act
			var result = WithMines();

			var mineList = mines.ToList();
			Assert.That(result == this);
			Assert.That(mineList.SingleOrDefault(m => m.X == 1 && m.Y == 1) != null);
			Assert.That(mineList.SingleOrDefault(m => m.X == 1 && m.Y == 3) != null);
			Assert.That(mineList.SingleOrDefault(m => m.X == 3 && m.Y == 3) != null);
		}

		[Test]
		public void WithExitPoint_ShouldThrowArgumentOutOfRangeExceptionInCaseOfInvalidInput()
		{
			inputExitCoordinates = "1 3 3";

			Assert.Throws<ArgumentOutOfRangeException>(() => WithExitPoint());
		}

		[Test]
		public void WithExitPoint_ShouldThrowFormatExceptionInCaseOfInvalidInput()
		{
			inputExitCoordinates = "1,3 3";

			Assert.Throws<FormatException>(() => WithExitPoint());
		}


		[Test]
		public void WithExitPoint_ShouldReturnThisInCaseOfCorrectInput()
		{
			//Arrange
			inputExitCoordinates = "1 4";

			//Act
			var result = WithExitPoint();

			Assert.That(result == this);
			Assert.That(exitPoint.X == 4 && exitPoint.Y == 1);
		}

		[Test]
		public void WithTurtle_ShouldReturnThisInCaseOfCorrectInputAndNorthInCaseOf_N_Ending()
		{
			//Arrange
			inputStartingCoordinates = "1 4 N";

			//Act
			var result = WithTurtle();

			Assert.That(result == this);
			Assert.That(turtle.Tile.X == 4 && turtle.Tile.Y == 1);
			Assert.That(turtle.Direction == Direction.NorthDirection);
		}

		[Test]
		public void WithTurtle_ShouldReturnThisInCaseOfCorrectInputAndWestnCaseOf_W_Ending()
		{
			//Arrange
			inputStartingCoordinates = "0 0 W";

			//Act
			var result = WithTurtle();

			Assert.That(result == this);
			Assert.That(turtle.Tile.X == 0 && turtle.Tile.Y == 0);
			Assert.That(turtle.Direction == Direction.WestDirection);
		}

		[Test]
		public void WithTurtle_ShouldReturnThisInCaseOfCorrectInputAndEastInCaseOf_E_Ending()
		{
			//Arrange
			inputStartingCoordinates = "0 2 E";

			//Act
			var result = WithTurtle();

			Assert.That(result == this);
			Assert.That(turtle.Tile.X == 2 && turtle.Tile.Y == 0);
			Assert.That(turtle.Direction == Direction.EastDirection);
		}

		[Test]
		public void WithTurtle_ShouldThrowArgumentOutOfRangeExceptionInCaseOfInvalidInput()
		{
			//Arrange
			inputStartingCoordinates = "1 4N";

			//Act
			Assert.Throws<ArgumentOutOfRangeException>(() => WithTurtle());
		}


		[Test]
		public void WithTiles_ShouldReturnThisInCaseOfCorrect()
		{
			//Arrange
			inputBoardSize = "3 4";

			//Act
			var result = WithTiles();

			Assert.That(result == this);
			Assert.That(tiles.GetLength(1) == 3);
			Assert.That(tiles.GetLength(0) == 4);
		}


		[Test]
		public void WithTiles_ShouldThrowArgumentOutOfRangeExceptionInCaseOfInvalidInput()
		{
			//Arrange
			inputBoardSize = "1 5 4";

			//Act
			Assert.Throws<ArgumentOutOfRangeException>(() => WithTiles());
		}

		[Test]
		public void CreateDefault_ShouldMineFieldIfAllArgumentsAreNotNull()
		{
			//Arrange
			tiles = new Tile[3, 2];
			turtle = new Turtle(new Tile(1, 2), Direction.EastDirection);
			mines = new Mine[] { new Mine(1, 1) };
			exitPoint = new ExitPoint(0, 0);

			//Act
			var minefield = CreateDefault();

			Assert.That(minefield != null);
		}

		[Test]
		public void CreateDefault_ShouldThrowArgumentNullExceptionInCaseOfAnyNullArgument_ExitPoint()
		{
			//Arrange
			tiles = new Tile[3, 2];
			turtle = new Turtle(new Tile(1, 2), Direction.EastDirection);
			mines = new Mine[] { new Mine(1, 1) };
			exitPoint = null;

			//Act
			Assert.Throws<ArgumentNullException>(() => CreateDefault());
		}

		[Test]
		public void CreateDefault_ShouldThrowArgumentNullExceptionInCaseOfAnyNullArgument_Mines()
		{
			//Arrange
			tiles = new Tile[3, 2];
			turtle = new Turtle(new Tile(1, 2), Direction.EastDirection);
			mines = null;
			exitPoint = new ExitPoint(0,0);

			//Act
			Assert.Throws<ArgumentNullException>(() => CreateDefault());
		}

		[Test]
		public void CreateDefault_ShouldThrowArgumentNullExceptionInCaseOfAnyNullArgument_Turtle()
		{
			//Arrange
			tiles = new Tile[3, 2];
			turtle = null;
			mines = new Mine[] { new Mine(1, 1) };
			exitPoint = new ExitPoint(0, 0);

			//Act
			Assert.Throws<ArgumentNullException>(() => CreateDefault());
		}

		[Test]
		public void CreateDefault_ShouldThrowArgumentNullExceptionInCaseOfAnyNullArgument_Tiles()
		{
			//Arrange
			tiles = null;
			turtle = new Turtle(new Tile(1, 2), Direction.EastDirection);
			mines = new Mine[] { new Mine(1, 1) };
			exitPoint = new ExitPoint(0, 0);

			//Act
			Assert.Throws<ArgumentNullException>(() => CreateDefault());
		}
	}
}
