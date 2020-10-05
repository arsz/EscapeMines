using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeMines.Services
{
	public static class AsyncEnumeratorExtension
	{
		/// <summary>
		/// Here should be nice the TryGet pattern but async could not have out parameter... after the task returned, the "out" parameters are not provided
		/// </summary>
		/// <param name="asyncEnumerator"></param>
		/// <returns></returns>
		public static async Task<(bool HasNext, string Result)> GetNextLineContent(this IAsyncEnumerator<string> asyncEnumerator)
		{
			var hasNext = await asyncEnumerator.MoveNextAsync().ConfigureAwait(false);
			return (hasNext, asyncEnumerator.Current);
		}
	}
}
