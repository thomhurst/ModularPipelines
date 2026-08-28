using System.Runtime.InteropServices;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
using ModularPipelines.Context.Domains.Environment;
using ModularPipelines.Context.Domains.Files;
using ModularPipelines.Context.Domains.Installers;
using ModularPipelines.Context.Domains.Network;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Options;
using Moq;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests.Helpers;

public class PredefinedInstallersTests
{
    [Test]
    public async Task Chocolatey_Runs_PowerShell_Directly_And_Adds_Install_Directory_To_Path()
    {
        const string allUsersProfile = @"C:\ProgramData";
        CommandLineToolOptions? capturedOptions = null;
        CancellationToken capturedCancellationToken = default;
        using var cancellationTokenSource = new CancellationTokenSource();
        var result = CreateResult();
        var command = new Mock<ICommandContext>();
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                It.IsAny<CommandLineToolOptions>(),
                It.IsAny<CommandExecutionOptions?>(),
                It.IsAny<CancellationToken>()))
            .Callback<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken>(
                (options, _, cancellationToken) =>
                {
                    capturedOptions = options;
                    capturedCancellationToken = cancellationToken;
                })
            .ReturnsAsync(result);
        var environmentVariables = new Mock<IEnvironmentVariablesContext>();
        environmentVariables.Setup(context => context.Get(
                "ALLUSERSPROFILE",
                EnvironmentVariableTarget.Process))
            .Returns(allUsersProfile);
        var installer = CreateInstaller(
            command.Object,
            Mock.Of<IEnvironmentContext>(),
            Mock.Of<IDownloaderContext>(),
            Mock.Of<IBashContext>(),
            environmentVariables.Object);

        var actualResult = await installer.ChocolateyAsync(cancellationTokenSource.Token);

        var options = (GenericCommandLineToolOptions) capturedOptions!;
        var arguments = options.Arguments!.ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(actualResult).IsSameReferenceAs(result);
            await Assert.That(options.Tool).IsEqualTo("powershell.exe");
            await Assert.That(arguments).Contains("-Command");
            await Assert.That(arguments).DoesNotContain("&&");
            await Assert.That(arguments).DoesNotContain("SET");
            await Assert.That(capturedCancellationToken).IsEqualTo(cancellationTokenSource.Token);
        }

        environmentVariables.Verify(context => context.AddToPath(
            Path.Combine(allUsersProfile, "chocolatey", "bin"),
            EnvironmentVariableTarget.Process), Times.Once);
    }

    [Test]
    public async Task Node_On_Unix_Sources_Nvm_And_Installs_In_One_Bash_Process()
    {
        var result = CreateResult();
        BashCommandOptions? capturedOptions = null;
        var environment = new Mock<IEnvironmentContext>();
        environment.SetupGet(context => context.OperatingSystem).Returns(OSPlatform.Linux);
        var downloadedScript = new File(Path.Combine(Path.GetTempPath(), "nvm-install.sh"));
        var downloader = new Mock<IDownloaderContext>();
        downloader.Setup(context => context.DownloadFileAsync(
                It.IsAny<DownloadFileOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(downloadedScript);
        var bash = new Mock<IBashContext>();
        bash.Setup(context => context.FromFileAsync(
                It.IsAny<BashFileOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        bash.Setup(context => context.CommandAsync(
                It.IsAny<BashCommandOptions>(),
                It.IsAny<CancellationToken>()))
            .Callback<BashCommandOptions, CancellationToken>((options, _) => capturedOptions = options)
            .ReturnsAsync(result);
        var installer = CreateInstaller(
            Mock.Of<ICommandContext>(),
            environment.Object,
            downloader.Object,
            bash.Object,
            Mock.Of<IEnvironmentVariablesContext>());

        var actualResult = await installer.NodeAsync("lts/iron");

        using (Assert.Multiple())
        {
            await Assert.That(actualResult).IsSameReferenceAs(result);
            await Assert.That(capturedOptions!.Command).IsEqualTo(
                "export NVM_DIR=\"$HOME/.nvm\" && [ -s \"$NVM_DIR/nvm.sh\" ] && . \"$NVM_DIR/nvm.sh\" && nvm install 'lts/iron'");
        }

        bash.Verify(context => context.FromFileAsync(
            It.Is<BashFileOptions>(options => options.FilePath == downloadedScript.Path),
            It.IsAny<CancellationToken>()), Times.Once);
        bash.Verify(context => context.CommandAsync(
            It.IsAny<BashCommandOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    private static PredefinedInstallers CreateInstaller(
        ICommandContext command,
        IEnvironmentContext environment,
        IDownloaderContext downloader,
        IBashContext bash,
        IEnvironmentVariablesContext environmentVariables)
    {
        return new PredefinedInstallers(
            command,
            environment,
            downloader,
            Mock.Of<IMacInstallerContext>(),
            Mock.Of<IWindowsInstallerContext>(),
            Mock.Of<ILinuxInstallerContext>(),
            bash,
            Mock.Of<IZipContext>(),
            environmentVariables);
    }

    private static CommandResult CreateResult()
    {
        var timestamp = DateTimeOffset.UtcNow;
        return new CommandResult(
            "installer",
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
