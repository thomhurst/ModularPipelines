using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Implementations;
using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.Context;

public class FilesContextTests
{
    [Test]
    [Arguments("")]
    [Arguments(" ")]
    public async Task GetFile_Throws_For_Blank_Path(string path)
    {
        var context = CreateContext();

        await Assert.That(() => context.GetFile(path)).Throws<ArgumentException>();
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    public async Task GetFolder_Throws_For_Blank_Path(string path)
    {
        var context = CreateContext();

        await Assert.That(() => context.GetFolder(path)).Throws<ArgumentException>();
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    public async Task ReadAsync_Throws_For_Blank_Path(string path)
    {
        var context = CreateContext();

        await Assert.ThrowsAsync<ArgumentException>(() => context.ReadAsync(path));
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    public async Task WriteAsync_Throws_For_Blank_Path(string path)
    {
        var context = CreateContext();

        await Assert.ThrowsAsync<ArgumentException>(() => context.WriteAsync(path, "content"));
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    public async Task Exists_Returns_False_For_Blank_Path(string path)
    {
        var context = CreateContext();

        await Assert.That(context.Exists(path)).IsFalse();
    }

    [Test]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(false, false)]
    public async Task Exists_Resolves_Files_And_Directories_Against_WorkingDirectory(bool fileExists, bool directoryExists)
    {
        var root = TestContext.OutputDirectory!;
        var resolvedPath = Path.Combine(root, "entry");
        var provider = new Mock<IFileSystemProvider>();
        provider.Setup(x => x.FileExists(resolvedPath)).Returns(fileExists);
        provider.Setup(x => x.DirectoryExists(resolvedPath)).Returns(directoryExists);
        var context = new FilesContext(provider.Object, new PipelineWorkingDirectory(root), Mock.Of<IZipContext>());

        await Assert.That(context.Exists("entry")).IsEqualTo(fileExists || directoryExists);
    }

    private static FilesContext CreateContext() =>
        new(
            Mock.Of<IFileSystemProvider>(),
            new PipelineWorkingDirectory(TestContext.OutputDirectory!),
            Mock.Of<IZipContext>());
}
