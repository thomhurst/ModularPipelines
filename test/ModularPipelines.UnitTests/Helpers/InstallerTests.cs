using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
using ModularPipelines.Context.Domains.Network;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Options.Linux;
using ModularPipelines.Options.Windows;
using ModularPipelines.TestHelpers;
using TUnit.Core.Exceptions;

namespace ModularPipelines.UnitTests.Helpers;

public class InstallerTests : TestBase
{
    [Test]
    public async Task Public_Command_Methods_Are_Asynchronous()
    {
        var commandMethods = typeof(IModuleContext).Assembly.ExportedTypes
            .Where(type => type.IsInterface)
            .SelectMany(type => type.GetMethods())
            .Where(method => method.ReturnType == typeof(Task<CommandResult>))
            .ToList();

        await Assert.That(commandMethods).IsNotEmpty();
        await Assert.That(commandMethods.All(method =>
            method.Name.EndsWith("Async", StringComparison.Ordinal))).IsTrue();
    }

    [Test]
    [Skip("Avoid installing things on people's machines")]
    public async Task Can_Install()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") != "true")
        {
            throw new SkipTestException("Avoid installing things on people's machines");
        }

        var downloader = await GetService<IDownloaderContext>();
        var installer = await GetService<IInstallersContext>();

        if (OperatingSystem.IsWindows())
        {
            var uri = new Uri("https://github.com/peazip/PeaZip/releases/download/9.4.0/peazip-9.4.0.WIN64.exe");

            var file = await downloader.DownloadFileAsync(new DownloadFileOptions(uri));

            var result = await installer.Windows.InstallExeAsync(new ExeInstallerOptions(file));
            await Assert.That(result.ExitCode).IsEqualTo(0);
        }
        else if (OperatingSystem.IsLinux())
        {
            var uri = new Uri("https://github.com/peazip/PeaZip/releases/download/9.4.0/peazip_9.4.0.LINUX.Qt5-1_amd64.deb");

            var file = await downloader.DownloadFileAsync(new DownloadFileOptions(uri));

            var result = await installer.Linux.InstallFromDpkgAsync(new DpkgInstallOptions(file));
            await Assert.That(result.ExitCode).IsEqualTo(0);
        }
    }
}
