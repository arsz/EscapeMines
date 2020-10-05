using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeMines.GameParts
{
	public interface IMovementFactory
	{
		Task<Movement[]> GetNextMovementsAsync();
	}
}
