using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Logging;
using Moq;

namespace ModularPipelines.Git.UnitTests;

public class GitVersioningTests
{
    [Test]
    public async Task Constructor_Does_Not_Touch_The_File_System()
    {
        var fileSystemProvider = new Mock<IFileSystemProvider>(MockBehavior.Strict);

        _ = new GitVersioning(
            Mock.Of<IGitInformation>(),
            Mock.Of<ICommandContext>(),
            Mock.Of<IModuleLoggerAccessor>(),
            fileSystemProvider.Object);

        await Assert.That(fileSystemProvider.Invocations.Count).IsEqualTo(0);
    }

    [Test]
    public async Task Missing_Repository_Does_Not_Create_Temporary_Folder()
    {
        var fileSystemProvider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        var gitInformation = new Mock<IGitInformation>();
        gitInformation.Setup(x => x.GetInfoAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((GitRepositoryInfo?) null);

        var versioning = new GitVersioning(
            gitInformation.Object,
            Mock.Of<ICommandContext>(),
            Mock.Of<IModuleLoggerAccessor>(),
            fileSystemProvider.Object);

        await Assert.That(async () => { await versioning.GetVersioningInformationAsync(); })
            .Throws<InvalidOperationException>();
        await Assert.That(fileSystemProvider.Invocations.Count).IsEqualTo(0);
    }
}
