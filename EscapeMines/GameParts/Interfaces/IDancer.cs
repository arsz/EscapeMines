namespace EscapeMines.GameParts.Interfaces
{
	public interface IDancer
	{
		Tile Tile { get; }
		void Rotate(Movement movement);
		void Move(Movement movement, Tile[,] board);
	}
}
