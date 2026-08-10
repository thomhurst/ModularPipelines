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
    public async Task ExistsAsync_Returns_False_For_Blank_Path(string path)
    {
        var context = new FilesContext(
            Mock.Of<IFileSystemContext>(),
            Mock.Of<IFileSystemProvider>(),
            new PipelineWorkingDirectory(TestContext.OutputDirectory!),
            Mock.Of<IZipContext>(),
            Mock.Of<IChecksumContext>());

        await Assert.That(await context.ExistsAsync(path)).IsFalse();
    }
}
