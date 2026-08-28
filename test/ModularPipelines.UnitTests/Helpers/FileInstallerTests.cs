using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Implementations;
using ModularPipelines.Context.Domains.Network;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.UnitTests.Attributes;
using Moq;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests.Helpers;

public class InstallersContextTests
{
    [Test]
    public async Task WebInstallerDownloadsThenRunsFileWithArguments()
    {
        var uri = new Uri("https://example.test/installer");
        var downloadedFile = new File("downloaded-installer");
        var downloader = new Mock<IDownloaderContext>();
        downloader.Setup(context => context.DownloadFileAsync(
                It.Is<DownloadFileOptions>(options => options.DownloadUri == uri),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(downloadedFile);
        var installer = new RecordingInstallersContext(downloader.Object);
        var arguments = new[] { "--silent" };

        await installer.InstallFromWebAsync(new WebInstallerOptions(uri)
        {
            Arguments = arguments,
        });

        using (Assert.Multiple())
        {
            await Assert.That(installer.Options?.Path).IsEqualTo(downloadedFile.Path);
            await Assert.That(installer.Options?.Arguments).IsEquivalentTo(arguments);
        }
    }

    [Test]
    [LinuxOnlyTest]
    public async Task EscapesPathForMakeExecutableCommand()
    {
        const string path = "/tmp/it's ready.sh";
        BashCommandOptions? chmodOptions = null;
        BashFileOptions? scriptOptions = null;
        var result = CreateResult();
        var bash = new Mock<IBashContext>();
        bash.Setup(context => context.RunAsync(
                It.IsAny<BashCommandOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .Callback<BashCommandOptions, CommandExecutionOptions?, CancellationToken>(
                (options, _, _) => chmodOptions = options)
            .ReturnsAsync(result);
        bash.Setup(context => context.RunFileAsync(
                It.IsAny<BashFileOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .Callback<BashFileOptions, CommandExecutionOptions?, CancellationToken>(
                (options, _, _) => scriptOptions = options)
            .ReturnsAsync(result);
        var installer = new InstallersContext(
            Mock.Of<ICommandContext>(),
            Mock.Of<IDownloaderContext>(),
            bash.Object);

        await installer.InstallAsync(new InstallerOptions(path));

        using (Assert.Multiple())
        {
            await Assert.That(chmodOptions?.Command)
                .IsEqualTo("chmod u+x '/tmp/it'\\''s ready.sh'");
            await Assert.That(scriptOptions?.FilePath).IsEqualTo(path);
        }
    }

    private static CommandResult CreateResult()
    {
        var timestamp = DateTimeOffset.UtcNow;
        return new CommandResult(
            "bash",
            Environment.CurrentDirectory,
            string.Empty,
            string.Empty,
            new Dictionary<string, string?>(),
            timestamp,
            timestamp,
            TimeSpan.Zero,
            0);
    }

    private sealed class RecordingInstallersContext : InstallersContext
    {
        public RecordingInstallersContext(IDownloaderContext downloader)
            : base(Mock.Of<ICommandContext>(), downloader, Mock.Of<IBashContext>())
        {
        }

        public InstallerOptions? Options { get; private set; }

        public override Task<CommandResult> InstallAsync(
            InstallerOptions options,
            CancellationToken cancellationToken = default)
        {
            Options = options;
            return Task.FromResult(CreateResult());
        }
    }
}
