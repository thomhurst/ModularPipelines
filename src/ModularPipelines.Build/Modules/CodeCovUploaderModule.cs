using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Options.Windows;

namespace ModularPipelines.Build.Modules;

[DependsOn<MergeCoverageModule>]
[RunOnLinux]
[SkipIfNoGitHubToken]
[SkipIfNoStandardGitHubToken]
public class CodeCovUploaderModule : Module<CommandResult>
{
    private readonly IOptions<CodeCovSettings> _codeCovSettings;

    public CodeCovUploaderModule(IOptions<CodeCovSettings> codeCovSettings)
    {
        _codeCovSettings = codeCovSettings;
    }

    protected override Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        if (string.IsNullOrEmpty(_codeCovSettings.Value.Token))
        {
            return SkipDecision.Skip("No token to upload CodeCov results").AsTask();
        }

        return SkipDecision.DoNotSkip.AsTask();
    }

    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var coverageFileResult = await GetModule<MergeCoverageModule>();

        if (OperatingSystem.IsWindows())
        {
            var exeFile = await context.Downloader.DownloadFileAsync(
                new DownloadFileOptions(new Uri("https://uploader.codecov.io/latest/windows/codecov.exe")), cancellationToken);

            await context.Installer.WindowsInstaller.InstallExe(new ExeInstallerOptions(exeFile)
            {
                Arguments = ["-t", _codeCovSettings.Value.Token!],
            });
        }

        if (OperatingSystem.IsLinux())
        {
            await context.Bash.Command(new BashCommandOptions($"""
                                                               curl -Os https://uploader.codecov.io/latest/linux/codecov

                                                               chmod +x codecov
                                                               ./codecov -t {_codeCovSettings.Value.Token} -f {coverageFileResult.Value}
                                                               """), cancellationToken);
        }

        return await NothingAsync();
    }
}