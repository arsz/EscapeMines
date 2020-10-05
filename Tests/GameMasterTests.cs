using EscapeMines.GameParts;
using EscapeMines.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
	public class GameMasterTests
	{

		private Mock<IInputService> mockedInputService;
		private Mock<IBoardFactory> mockedBoardFactory;
		private CancellationToken testToken;
		public GameMaster gameMaster;


		[SetUp]
		public void Setup()
		{
			testToken = CancellationToken.None;
			mockedInputService = new Mock<IInputService>();
			mockedBoardFactory = new Mock<IBoardFactory>();
			gameMaster = new GameMaster(mockedInputService.Object, mockedBoardFactory.Object);
		}

		[Test]
		public async Task ShouldInvokeAllNecessaryFunctionInOrderToPlayTheGame()
		{
			//Arrange
			var testMineField = new Mock<IMineField>();
			var mockedFluentMineFluent = new Mock<IFluentMineField>();
			testMineField.Setup(x => x.IsStillMovingOnAsync()).Returns(Task.FromResult(false))
				.Callback(() => testMineField.Verify(x => x.FillUpBoard(),Times.Once));

			mockedFluentMineFluent.Setup(x => x.WithExitPoint()).Returns(mockedFluentMineFluent.Object);
			mockedFluentMineFluent.Setup(x => x.WithMines()).Returns(mockedFluentMineFluent.Object);
			mockedFluentMineFluent.Setup(x => x.WithTurtle()).Returns(mockedFluentMineFluent.Object);
			mockedFluentMineFluent.Setup(x => x.WithTiles()).Returns(mockedFluentMineFluent.Object);
			mockedFluentMineFluent.Setup(x => x.CreateDefault()).Returns(testMineField.Object);
			var mockedAsyncEnumerator = new Mock<IAsyncEnumerator<string>>();
			mockedAsyncEnumerator.Setup(x => x.MoveNextAsync()).Returns(new ValueTask<bool>(true));
			mockedInputService.Setup(x => x.ReadInputTextFileLinesAsync(testToken)).Returns(mockedAsyncEnumerator.Object);
			mockedBoardFactory.Setup(x => x.InitBoardAsync(mockedAsyncEnumerator.Object)).Returns(Task.FromResult(mockedFluentMineFluent.Object));

			//Act
			await gameMaster.StartTheProcessAsync(testToken);

			//Assert
			mockedInputService.Verify(x => x.ReadInputTextFileLinesAsync(testToken), Times.Once);
			mockedBoardFactory.Verify(x => x.InitBoardAsync(mockedAsyncEnumerator.Object), Times.Once);
			mockedFluentMineFluent.Verify(x => x.WithExitPoint(),Times.Once);
			mockedFluentMineFluent.Verify(x => x.WithMines(), Times.Once);
			mockedFluentMineFluent.Verify(x => x.WithTiles(), Times.Once);
			mockedFluentMineFluent.Verify(x => x.WithTurtle(), Times.Once);
			mockedFluentMineFluent.Verify(x => x.CreateDefault(), Times.Once);
			testMineField.Verify(x => x.IsStillMovingOnAsync(), Times.Once);
			testMineField.Verify(x => x.GetResult(), Times.Once);
			
		}

		[Test]
		public async Task ShouldDisposeInputAfterGameHasFinished()
		{
			//Arrange
			var testMineField = new Mock<IMineField>();
			var mockedFluentMineFluent = new Mock<IFluentMineField>();
			testMineField.Setup(x => x.IsStillMovingOnAsync()).Returns(Task.FromResult(false));
			mockedFluentMineFluent.Setup(x => x.WithExitPoint()).Returns(mockedFluentMineFluent.Object);
			mockedFluentMineFluent.Setup(x => x.WithMines()).Returns(mockedFluentMineFluent.Object);
			mockedFluentMineFluent.Setup(x => x.WithTurtle()).Returns(mockedFluentMineFluent.Object);
			mockedFluentMineFluent.Setup(x => x.WithTiles()).Returns(mockedFluentMineFluent.Object);
			mockedFluentMineFluent.Setup(x => x.CreateDefault()).Returns(testMineField.Object);
			var mockedAsyncEnumerator = new Mock<IAsyncEnumerator<string>>();
			mockedAsyncEnumerator.Setup(x => x.MoveNextAsync()).Returns(new ValueTask<bool>(true));
			mockedInputService.Setup(x => x.ReadInputTextFileLinesAsync(testToken)).Returns(mockedAsyncEnumerator.Object);
			mockedBoardFactory.Setup(x => x.InitBoardAsync(mockedAsyncEnumerator.Object)).Returns(Task.FromResult(mockedFluentMineFluent.Object));

			testMineField.Setup(x => x.GetResult()).Callback(() => mockedAsyncEnumerator.Verify(x => x.DisposeAsync(), Times.Never));

			//Act
			await gameMaster.StartTheProcessAsync(testToken);

			//Assert
			mockedAsyncEnumerator.Verify(x => x.DisposeAsync(), Times.Once);
		}
	}
}
