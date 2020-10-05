using EscapeMines.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EscapeMines.GameParts
{
	public class GameMaster
    {
        private readonly IInputService inputService;
		private readonly IBoardFactory boardFactory;

		public GameMaster(IInputService inputService, IBoardFactory boardFactory)
        {
            this.inputService = inputService;
			this.boardFactory = boardFactory;
		}

        public async Task<string> StartTheProcessAsync(CancellationToken cancellationToken)
        {
            var contentAsyncEnumerator = inputService.ReadInputTextFileLinesAsync(cancellationToken);

            try
			{
                var mineField = await CreateBoardForPlayingAsync(contentAsyncEnumerator).ConfigureAwait(false);

                bool isGameInProgress = true;

                while (isGameInProgress)
                {
                    isGameInProgress = await mineField.IsStillMovingOnAsync().ConfigureAwait(false);
                }

                return mineField.GetResult();
            }
			finally
			{
                await contentAsyncEnumerator.DisposeAsync();
            }
        }

        private async Task<IMineField> CreateBoardForPlayingAsync(IAsyncEnumerator<string> contentLinesAsyncEnumerator)
		{
             var board = await boardFactory.InitBoardAsync(contentLinesAsyncEnumerator).ConfigureAwait(false);

            var mineField = board.WithMines()
                                    .WithTiles()
                                    .WithTurtle()
                                    .WithExitPoint()
                                    .CreateDefault();

            mineField.FillUpBoard(); 

            return mineField;
        }
    }
}
