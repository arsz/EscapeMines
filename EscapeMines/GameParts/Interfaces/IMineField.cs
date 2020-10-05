using System.Threading.Tasks;

namespace EscapeMines.GameParts
{
	public interface IMineField
	{
		Task<bool> IsStillMovingOnAsync();

		string GetResult();

		void FillUpBoard();
	}
}
