using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Network;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.UnitTests.Attributes;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class FileInstallerTests
{
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
        var installer = new FileInstaller(
            Mock.Of<ICommandContext>(),
            Mock.Of<IDownloaderContext>(),
            bash.Object);

        await installer.InstallFromFileAsync(new InstallerOptions(path));

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
}
