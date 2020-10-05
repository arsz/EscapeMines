using EscapeMines.GameParts;
using EscapeMines.Services;
using System;
using System.Threading;

namespace EscapeMines
{
	class Program
    {
        static void Main(string[] args)
        {
            //Depdendencies that should be provided by a DI container...
            var fileDialogOpener = new FileDialogOpener();
            var fileReader = new FileReader();
            var fileWrapper = new FileWrapper();
            var inputService = new InputService(fileDialogOpener, fileReader, fileWrapper);
            var boardFactory = new GameFactory();

            Start(inputService,boardFactory);

            Console.ReadKey();
        }

        public static async void Start(InputService inputService, IBoardFactory boardFactory)
		{
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
            {
                var game = new GameMaster(inputService, boardFactory);

                var result = await game.StartTheProcessAsync(cancellationTokenSource.Token).ConfigureAwait(false);

                Console.WriteLine(result);
            }
        }
    }
}
