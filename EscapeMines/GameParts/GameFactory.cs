using EscapeMines.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscapeMines.GameParts
{
	public class GameFactory : IFluentMineField, IBoardFactory, IMovementFactory
	{
		protected string inputBoardSize;
		protected string inputMinesCoordinates;
		protected string inputExitCoordinates;
		protected string inputStartingCoordinates;

		protected Tile[,] tiles;
		protected Turtle turtle;
		protected ExitPoint exitPoint;
		protected Mine[] mines;

		protected IAsyncEnumerator<string> asyncInputEnumerator;

		private async Task<string> GetNextBoardInputAsync()
		{
			var tupleResult = await asyncInputEnumerator.GetNextLineContent().ConfigureAwait(false);

			if (tupleResult.HasNext == false) throw new ArgumentException("Not enough input data.");

			return tupleResult.Result;
		}

		private async Task<string> GetNextMovementInputAsync()
		{
			var tupleResult = await asyncInputEnumerator.GetNextLineContent().ConfigureAwait(false);

			if (tupleResult.HasNext == false) return string.Empty;

			return tupleResult.Result;
		}

		public async Task<Movement[]> GetNextMovementsAsync()
		{
			var nextLineMovements = await GetNextMovementInputAsync();

			if (string.IsNullOrEmpty(nextLineMovements)) return Array.Empty<Movement>();

			var allMovementsInOneLine = new List<Movement>();
			foreach (char movementSign in nextLineMovements)
			{
				if (char.IsWhiteSpace(movementSign)) continue;

				var convertedMovementEnum = (Movement)Enum.ToObject(typeof(Movement), movementSign);
				allMovementsInOneLine.Add(convertedMovementEnum);
			}

			return allMovementsInOneLine.ToArray();
		}

		public async Task<IFluentMineField> InitBoardAsync(IAsyncEnumerator<string> asyncInputEnumerator)
		{
			this.asyncInputEnumerator = asyncInputEnumerator;

			inputBoardSize = await GetNextBoardInputAsync().ConfigureAwait(false);
			inputMinesCoordinates = await GetNextBoardInputAsync().ConfigureAwait(false);
			inputExitCoordinates = await GetNextBoardInputAsync().ConfigureAwait(false);
			inputStartingCoordinates = await GetNextBoardInputAsync().ConfigureAwait(false);
			
			return this;
		}

		public IFluentMineField WithMines()
		{
			var coordiantesForEachMines = inputMinesCoordinates.Split(' ');
			var mines = new List<Mine>();

			foreach (var coordinates in coordiantesForEachMines)
			{
				var xy = coordinates.Split(',');

				if (xy.Length != 2) throw new ArgumentOutOfRangeException("Invalid mines coordinates.");

				var x = int.Parse(xy[0]);
				var y = int.Parse(xy[1]);
				mines.Add(new Mine(x, y));
			}

			this.mines = mines.ToArray();

			return this;
		}

		public IFluentMineField WithExitPoint()
		{
			var exitCoordinates = inputExitCoordinates.Split(' ');
			if (exitCoordinates.Length != 2) throw new ArgumentOutOfRangeException("Invalid exit parameters.");

			var y = int.Parse(exitCoordinates[0]);
			var x = int.Parse(exitCoordinates[1]);

			exitPoint = new ExitPoint(x, y);

			return this;
		}

		public IFluentMineField WithTiles()
		{
			var boardSize = inputBoardSize.Split(' ');
			if (boardSize.Length != 2) throw new ArgumentOutOfRangeException("Invalid boardSize parameters.");

			var y = int.Parse(boardSize[0]);
			var x = int.Parse(boardSize[1]);

			tiles = new Tile[x, y];

			return this;
		}

		public IFluentMineField WithTurtle()
		{
			var turtleArguments = inputStartingCoordinates.Split(' ');
			if (turtleArguments.Length != 3) throw new ArgumentOutOfRangeException("Invalid turtle arguments.");

			var y = int.Parse(turtleArguments[0]);
			var x = int.Parse(turtleArguments[1]);
			var startingTile = new Tile(x, y);

			var directionSign = turtleArguments[2].First();
			var direction = (Direction)Enum.ToObject(typeof(Direction), directionSign);

			turtle = new Turtle(startingTile, direction);

			return this;
		}


		public IMineField CreateDefault()
		{
			if (mines == null || exitPoint == null || turtle == null || tiles == null) throw new ArgumentNullException("Board is not ready for playing!");

			return new Minefield(this, tiles, mines, exitPoint, turtle);
		}
	}
}
