using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.FileSystem;
using ModularPipelines.Logging;
using Moq;

namespace ModularPipelines.Git.UnitTests;

public class GitVersioningTests
{
    [Test]
    public void Constructor_Creates_Temporary_Folder_Through_Configured_Provider()
    {
        const string temporaryRoot = "virtual-temp";
        const string randomName = "gitversion.tmp";
        const string temporaryFolder = "virtual-temp/gitversiontmp";
        var fileSystemProvider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        fileSystemProvider.Setup(x => x.GetTempPath()).Returns(temporaryRoot);
        fileSystemProvider.Setup(x => x.GetRandomFileName()).Returns(randomName);
        fileSystemProvider.Setup(x => x.Combine(temporaryRoot, "gitversiontmp"))
            .Returns(temporaryFolder);
        fileSystemProvider.Setup(x => x.CreateDirectory(temporaryFolder));

        _ = new GitVersioning(
            Mock.Of<IGitInformation>(),
            Mock.Of<ICommandContext>(),
            Mock.Of<IModuleLoggerProvider>(),
            fileSystemProvider.Object);

        fileSystemProvider.VerifyAll();
    }
}
