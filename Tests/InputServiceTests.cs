using EscapeMines.Services;
using Moq;
using NUnit.Framework;
using System.IO;
using System.Threading;

namespace Tests
{
	public class InputServiceTests
	{
		private Mock<IFileReader> mockedFileReader;
		private Mock<IFileDialogOpener> mockedFileDialogOpener;
		private Mock<IFileWrapper> mockedFileWrapper;
		private IInputService inputService;
		private CancellationToken testToken;

		[SetUp]
		public void Setup()
		{
			testToken = CancellationToken.None;
			mockedFileReader = new Mock<IFileReader>();
			mockedFileDialogOpener = new Mock<IFileDialogOpener>();
			mockedFileWrapper = new Mock<IFileWrapper>();
			inputService = new InputService(mockedFileDialogOpener.Object, mockedFileReader.Object, mockedFileWrapper.Object);
		}

		[Test]
		public void ShouldThrowFileLoadExceptionIfGetFilePathFromDialogReturnsWithEmpty()
		{
			//Arrange
			mockedFileDialogOpener.Setup(x => x.GetFilePathFromDialog()).Returns(string.Empty);

			//Act - Assert
			Assert.Throws<FileLoadException>(() => inputService.ReadInputTextFileLinesAsync(testToken));

			//Assert
			mockedFileReader.Verify(x => x.ReadLinesAsync(It.IsAny<string>()), Times.Never);
		}

		[Test]
		public void ShouldThrowFileNotFoundExceptionIfGetFilePathFromDialogReturnsNonExistingFilePath()
		{
			//Arrange
			var nonexistingfilepath = "testPath";
			mockedFileDialogOpener.Setup(x => x.GetFilePathFromDialog()).Returns(nonexistingfilepath);
			mockedFileWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);

			//Act - Assert
			Assert.Throws<FileNotFoundException>(() => inputService.ReadInputTextFileLinesAsync(testToken));

			//Assert
			mockedFileReader.Verify(x => x.ReadLinesAsync(It.IsAny<string>()), Times.Never);
		}

		[Test]
		public void ShouldCallReadLinesAsyncSuccessfullyOnceIfFileExists()
		{
			//Arrange
			var existingPath = "testPath";
			mockedFileDialogOpener.Setup(x => x.GetFilePathFromDialog()).Returns(existingPath);
			mockedFileWrapper.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
			mockedFileReader.Setup(x => x.ReadLinesAsync(existingPath).GetAsyncEnumerator(testToken));

			//Act
			inputService.ReadInputTextFileLinesAsync(testToken);

			//Assert
			mockedFileReader.Verify(x => x.ReadLinesAsync(existingPath), Times.Once);
		}

	}
}
