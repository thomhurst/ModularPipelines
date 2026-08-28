using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Files;
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
    public async Task ExistsAsync_Returns_False_For_Blank_Path(string path)
    {
        var context = CreateContext();

        await Assert.That(await context.ExistsAsync(path)).IsFalse();
    }

    private static FilesContext CreateContext() =>
        new(
            Mock.Of<IFileSystemProvider>(),
            new PipelineWorkingDirectory(TestContext.OutputDirectory!),
            Mock.Of<IZipContext>(),
            Mock.Of<IChecksumContext>());
}
