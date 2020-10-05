using EscapeMines.GameParts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscapeMines.GameParts
{
	public class Minefield : IMineField
	{
		private readonly IMovementFactory movementFactory;
		private readonly Tile[,] board;
		private readonly Mine[] mines;
		private readonly ExitPoint exitPoint;
		private readonly IDancer turtle;


		public Minefield(IMovementFactory movementFactory, Tile[,] board, Mine[] mines, ExitPoint exitPoint, IDancer turtle)
		{
			this.movementFactory = movementFactory;
			this.board = board;
			this.mines = mines;
			this.exitPoint = exitPoint;
			this.turtle = turtle;
		}

		public string GetResult() => turtle.Tile.GetStatus();

		public async Task<bool> IsStillMovingOnAsync()
		{
			var movements = await movementFactory.GetNextMovementsAsync();

			if (movements.Length == 0) return false;

			foreach (var movement in movements)
			{
				if (movement != Movement.Move)
				{
					turtle.Rotate(movement);
					continue;
				}

				turtle.Move(movement, board);

				if (turtle.Tile is Mine || turtle.Tile is ExitPoint) return false;
			}

			return true;
		}

		public void FillUpBoard()
		{
			FillUpExitPoint();
			FillUpMines();
			FillUpRemainingTiles();
		}

		private void FillUpExitPoint() => board[exitPoint.X, exitPoint.Y] = exitPoint;

		private void FillUpMines()
		{
			foreach (var mine in mines)
			{
				board[mine.X, mine.Y] = mine;
			}
		}

		private void FillUpRemainingTiles()
		{
			for (int i = 0; i < board.GetLength(0); i++)
			{
				for (int j = 0; j < board.GetLength(1); j++)
				{
					if (board[i, j] == null)
					{
						board[i, j] = new Tile(i, j);
					}
				}
			}
		}
	}
}
