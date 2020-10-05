using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EscapeMines.Services
{
    public interface IInputService
    {
        IAsyncEnumerator<string> ReadInputTextFileLinesAsync(CancellationToken cancellationToken);
    }
}