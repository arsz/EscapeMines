using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EscapeMines.Services
{
    public class InputService : IInputService
    {
        private readonly IFileDialogOpener fileDialogOpener;
        private readonly IFileReader fileReader;
		private readonly IFileWrapper fileWrapper;

		public InputService(IFileDialogOpener fileDialogOpener, IFileReader fileReader,IFileWrapper fileWrapper)
        {
            this.fileDialogOpener = fileDialogOpener;
            this.fileReader = fileReader;
			this.fileWrapper = fileWrapper;
		}

        public IAsyncEnumerator<string> ReadInputTextFileLinesAsync(CancellationToken cancellationToken)
        {
            var filePath = fileDialogOpener.GetFilePathFromDialog();

            if(string.IsNullOrEmpty(filePath)) throw new FileLoadException("Input cannot be loaded.");

            if (fileWrapper.Exists(filePath) == false) throw new FileNotFoundException("Input file not found.");

            return fileReader.ReadLinesAsync(filePath).GetAsyncEnumerator(cancellationToken);
        }
    }
}
