using System.Collections.Generic;
using System.Threading.Tasks;

namespace EscapeMines.Services
{
    public interface IFileReader
    {
        IAsyncEnumerable<string> ReadLinesAsync(string path);
    }
}