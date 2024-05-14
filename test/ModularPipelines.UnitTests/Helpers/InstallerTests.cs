using ModularPipelines.Context;
using ModularPipelines.Options;
using ModularPipelines.Options.Linux;
using ModularPipelines.Options.Windows;
using ModularPipelines.TestHelpers;
using TUnit.Assertions.Extensions.Is;
using TUnit.Core.Exceptions;

namespace ModularPipelines.UnitTests.Helpers;

public class InstallerTests : TestBase
{
    [Test]
    public async Task Can_Install()
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") != "true")
        {
            throw new SkipTestException("Avoid installing things on people's machines");
        }
        
        var downloader = await GetService<IDownloader>();
        var installer = await GetService<IInstaller>();

        if (OperatingSystem.IsWindows())
        {
            var uri = new Uri("https://github.com/peazip/PeaZip/releases/download/9.4.0/peazip-9.4.0.WIN64.exe");

            var file = await downloader.DownloadFileAsync(new DownloadFileOptions(uri));

            var result = await installer.WindowsInstaller.InstallExe(new ExeInstallerOptions(file));
            await Assert.That(result.ExitCode).Is.Zero();
        }
        else if (OperatingSystem.IsLinux())
        {
            var uri = new Uri("https://github.com/peazip/PeaZip/releases/download/9.4.0/peazip_9.4.0.LINUX.Qt5-1_amd64.deb");

            var file = await downloader.DownloadFileAsync(new DownloadFileOptions(uri));

            var result = await installer.LinuxInstaller.InstallFromDpkg(new DpkgInstallOptions(file));
            await Assert.That(result.ExitCode).Is.Zero();
        }
    }
}