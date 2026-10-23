using System.Net;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Implementations;
using ModularPipelines.FileSystem;
using ModularPipelines.Logging;
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
    [Arguments(".msi")]
    [Arguments(".MSI")]
    [Arguments(".ps1")]
    [Arguments(".PS1")]
    [Arguments(".exe")]
    [Arguments(".cmd")]
    public async Task InstallSelectsLauncherAndPreservesArguments(string extension)
    {
        var directory = Directory.CreateTempSubdirectory("installer test's ");
        var path = Path.Combine(directory.FullName, "installer" + extension);
        await File.WriteAllTextAsync(path, string.Empty);
        using var cancellation = new CancellationTokenSource();
        var arguments = new[] { "/quiet", "argument with spaces", "it's ready", "a&b;$value" };
        var isMsi = OperatingSystem.IsWindows() && extension.Equals(".msi", StringComparison.OrdinalIgnoreCase);
        var isPowerShell = extension.Equals(".ps1", StringComparison.OrdinalIgnoreCase);
        var expectedTool = isMsi ? "msiexec" : isPowerShell ? "pwsh" : path;
        var expectedArguments = isMsi ? new[] { "/i", path }.Concat(arguments)
            : isPowerShell ? new[] { "-NoLogo", "-NoProfile", "-NonInteractive", "-File", path }.Concat(arguments)
            : arguments;
        var result = CommandResult.Ok();
        var command = new Mock<ICommandContext>(MockBehavior.Strict);
        command.Setup(context => context.ExecuteCommandLineToolAsync(
                It.Is<CommandLineToolOptions>(options => options.Tool == expectedTool
                    && options.Arguments != null && options.Arguments.SequenceEqual(expectedArguments)),
                It.Is<CommandExecutionOptions?>(options => isMsi
                    ? options != null && options.SuccessExitCodes.SequenceEqual(new[] { 0, 1641, 3010 })
                    : options == null), cancellation.Token))
            .ReturnsAsync(result);
        var installer = new InstallersContext(
            command.Object, Mock.Of<IDownloaderContext>(), new PipelineWorkingDirectory(directory.FullName));

        try
        {
            var actual = await installer.InstallAsync(new InstallerOptions("./installer" + extension)
            {
                Arguments = arguments,
            }, cancellation.Token);

            await Assert.That(actual).IsSameReferenceAs(result);
            command.VerifyAll();
        }
        finally
        {
            File.Delete(path);
            directory.Delete();
        }
    }

    [Test]
    [RequiresTool("pwsh")]
    public async Task PowerShellInstallerRunsScriptWithLiteralArguments()
    {
        var directory = Directory.CreateTempSubdirectory("installer test's ");
        var path = Path.Combine(directory.FullName, "setup.ps1");
        try
        {
            await File.WriteAllTextAsync(path, "[Console]::WriteLine(($args -join '|'))");
            var installer = await GetService<IInstallersContext>();
            var result = await installer.InstallAsync(new InstallerOptions(path)
            {
                Arguments = ["argument with spaces", "it's ready", "a&b;$value"],
            });

            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.StandardOutput.TrimEnd()).IsEqualTo("argument with spaces|it's ready|a&b;$value");
        }
        finally
        {
            File.Delete(path);
            directory.Delete();
        }
    }

    [Test]
    [RequiresTool("pwsh")]
    [Arguments(false)]
    [Arguments(true)]
    public async Task WebPowerShellInstallerUsesResponseMetadataAndDeletesDownload(bool useContentDisposition)
    {
        using var finalRequest = new HttpRequestMessage(HttpMethod.Get,
            useContentDisposition ? "https://example.test/download" : "https://example.test/setup.ps1");
        var content = new StringContent("[Console]::WriteLine($PSCommandPath); [Console]::WriteLine(($args -join '|'))");
        if (useContentDisposition)
        {
            content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileNameStar = "setup.ps1",
            };
        }

        var http = new Mock<IHttpContext>();
        http.Setup(context => context.SendAsync(It.IsAny<HttpOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content,
                RequestMessage = finalRequest,
            });
        var logger = new Mock<IModuleLoggerAccessor>();
        logger.SetupGet(accessor => accessor.Logger).Returns(Mock.Of<IModuleLogger>());
        var workingDirectory = new PipelineWorkingDirectory(Environment.CurrentDirectory);
        var downloader = new Downloader(logger.Object, http.Object, SystemFileSystemProvider.Instance, workingDirectory,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        var installer = new InstallersContext(await GetService<ICommandContext>(), downloader, workingDirectory);

        var result = await installer.InstallFromWebAsync(new WebInstallerOptions(new Uri("https://example.test/install"))
        {
            Arguments = ["argument with spaces", "it's ready", "a&b;$value"],
        });

        var output = result.StandardOutput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        await Assert.That(result.ExitCode).IsEqualTo(0);
        await Assert.That(output.Length).IsEqualTo(2);
        await Assert.That(Path.GetExtension(output[0])).IsEqualTo(".ps1");
        await Assert.That(File.Exists(output[0])).IsFalse();
        await Assert.That(output[1]).IsEqualTo("argument with spaces|it's ready|a&b;$value");
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
            Skip.Test("Unix only test");
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
