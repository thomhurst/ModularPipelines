using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Implementations;
using ModularPipelines.FileSystem;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Attributes;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class InstallersContextTests : TestBase
{
    [Test]
    [Arguments("success")]
    [Arguments("failure")]
    [Arguments("cancellation")]
    public async Task WebInstallerDeletesDownloadAfterInstallation(string outcome)
    {
        var uri = new Uri("https://example.test/installer");
        var downloadedFile = new FilePath(Path.GetTempFileName());
        using var cancellation = new CancellationTokenSource();
        var token = cancellation.Token;
        var arguments = new[] { "--silent", "argument with spaces" };
        var result = CommandResult.Ok();
        Exception? failure = outcome switch
        {
            "failure" => new IOException("Installation failed"),
            "cancellation" => new OperationCanceledException(token),
            _ => null,
        };
        var downloader = new Mock<IDownloaderContext>();
        downloader.Setup(context => context.DownloadFileAsync(
                It.Is<DownloadFileOptions>(options => options.DownloadUri == uri), token))
            .ReturnsAsync(downloadedFile);
        var installer = new Mock<InstallersContext>(
            Mock.Of<ICommandContext>(), downloader.Object, new PipelineWorkingDirectory(Environment.CurrentDirectory))
        {
            CallBase = true,
        };
        installer.Setup(context => context.InstallAsync(
                It.Is<InstallerOptions>(options => options.Path == downloadedFile.Path
                    && options.Arguments == arguments), token))
            .Returns(async () =>
            {
                await Task.Yield();
                await Assert.That(downloadedFile.Exists).IsTrue();
                if (failure is not null)
                {
                    if (failure is OperationCanceledException)
                    {
                        cancellation.Cancel();
                    }

                    throw failure;
                }

                return result;
            });

        try
        {
            var installation = installer.Object.InstallFromWebAsync(new WebInstallerOptions(uri)
            {
                Arguments = arguments,
            }, token);

            if (failure is null)
            {
                await Assert.That(await installation).IsSameReferenceAs(result);
            }
            else
            {
                var exception = await Assert.ThrowsAsync<Exception>(() => installation);
                await Assert.That(exception).IsSameReferenceAs(failure);
            }

            await Assert.That(downloadedFile.Exists).IsFalse();
            installer.VerifyAll();
            downloader.VerifyAll();
        }
        finally
        {
            downloadedFile.Delete();
        }
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(true, true)]
    public async Task InstallExecutesFileDirectlyWithArgumentsAndCancellation(bool withArguments, bool relativePath)
    {
        var directory = Directory.CreateTempSubdirectory("installer test's ");
        var path = Path.Combine(directory.FullName, "installer");
        await File.WriteAllTextAsync(path, string.Empty);
        using var cancellation = new CancellationTokenSource();
        var arguments = withArguments ? new[] { "--silent", "argument with spaces", "it's ready" } : null;
        var result = CommandResult.Ok();
        var command = new Mock<ICommandContext>(MockBehavior.Strict);
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                It.Is<CommandLineToolOptions>(options => options.Tool == path
                    && options.Arguments != null
                    && options.Arguments.SequenceEqual(arguments ?? Array.Empty<string>())),
                null, cancellation.Token))
            .ReturnsAsync(result);
        var installer = new InstallersContext(
            command.Object, Mock.Of<IDownloaderContext>(), new PipelineWorkingDirectory(directory.FullName));

        try
        {
            if (!OperatingSystem.IsWindows())
            {
                File.SetUnixFileMode(path, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.GroupRead);
            }

            var actual = await installer.InstallAsync(new InstallerOptions(relativePath ? "./installer" : path)
            {
                Arguments = arguments,
            }, cancellation.Token);

            await Assert.That(actual).IsSameReferenceAs(result);
            await Assert.That(File.Exists(path)).IsTrue();
            command.VerifyAll();
            if (!OperatingSystem.IsWindows())
            {
                await Assert.That(File.GetUnixFileMode(path)).IsEqualTo(
                    UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute | UnixFileMode.GroupRead);
            }
        }
        finally
        {
            File.Delete(path);
            directory.Delete();
        }
    }

    [Test]
    public async Task PrecancelledInstallerDoesNotChangePermissionsOrExecuteCommand()
    {
        var path = Path.GetTempFileName();
        var mode = UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.GroupRead;
        var command = new Mock<ICommandContext>(MockBehavior.Strict);
        var installer = new InstallersContext(
            command.Object, Mock.Of<IDownloaderContext>(), new PipelineWorkingDirectory(Environment.CurrentDirectory));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        try
        {
            if (!OperatingSystem.IsWindows())
            {
                File.SetUnixFileMode(path, mode);
            }

            var exception = await Assert.ThrowsAsync<OperationCanceledException>(() =>
                installer.InstallAsync(new InstallerOptions(path), cancellation.Token));

            await Assert.That(exception!.CancellationToken).IsEqualTo(cancellation.Token);
            command.VerifyNoOtherCalls();
            if (!OperatingSystem.IsWindows())
            {
                await Assert.That(File.GetUnixFileMode(path)).IsEqualTo(mode);
            }
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Test]
    [UnixOnlyTest]
    [Arguments(false)]
    [Arguments(true)]
    public async Task UnixInstallerRunsNativeBinaryOrShebangScript(bool nativeBinary)
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var directory = Directory.CreateTempSubdirectory("installer test's ");
        var path = Path.Combine(directory.FullName, "installer");
        try
        {
            if (nativeBinary)
            {
                File.Copy("/bin/echo", path);
            }
            else
            {
                await File.WriteAllTextAsync(path, "#!/bin/sh\nprintf '%s %s\\n' \"$1\" \"$2\"\n");
            }

            File.SetUnixFileMode(path, UnixFileMode.UserRead | UnixFileMode.UserWrite);
            var installer = await GetService<IInstallersContext>();

            var result = await installer.InstallAsync(new InstallerOptions(path)
            {
                Arguments = ["argument with spaces", "it's ready"],
            });

            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.StandardOutput.TrimEnd()).IsEqualTo("argument with spaces it's ready");
        }
        finally
        {
            File.Delete(path);
            directory.Delete();
        }
    }
}
