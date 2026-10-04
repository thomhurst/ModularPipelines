using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Node.Options;

namespace ModularPipelines.Build.Modules;

[RequiresCapability("ci-master")]
[RunIf<ModularPipelines.OnLinux>]
[DependsOn<GenerateReadMeModule>]
public class FormatMarkdownModule : Module<None>
{
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithSkipWhen(ctx =>
        {
            if (FastFailValidation.IsComplete(ctx))
            {
                return SkipDecision.Skip("Validated by the fast-fail CI job");
            }

            if (ctx.Tools.GitHub.EnvironmentVariables.EventName != "pull_request")
            {
                return SkipDecision.Skip("Not a pull request");
            }

            return SkipDecision.DoNotSkip;
        })
        .WithAlwaysRun();

    protected override async Task<None> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await context.Tools.Npm.InstallAsync(new NpmInstallOptions
        {
            PackageSpec =
            [
                "remark-cli",
                "remark-preset-lint-consistent",
                "remark-preset-lint-recommended",
                "remark-lint-list-item-indent"
            ],
            SaveDev = true,
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        var repositoryInfo = await context.Tools.Git.Information.GetInfoAsync(cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");
        var filesToFormat = new List<string>
        {
            repositoryInfo.Root.FindFile(x => x.Name == "README.md")!.Path,
            repositoryInfo.Root.FindFile(x => x.Name == "README_Template.md")!.Path,
        };

        foreach (var fileToFormat in filesToFormat)
        {
            await context.Tools.Npx.ExecuteAsync(new NpxExecuteOptions
            {
                Pkg = "remark",
                Args =
                [
                    fileToFormat,
                    "--use", "remark-lint",
                    "--use", "remark-preset-lint-consistent",
                    "--use", "remark-preset-lint-recommended",
                    "--output"
                ],
            }, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        var changes = await GitHelpers.GetUncommittedChanges(context, filesToFormat, cancellationToken)
            .ConfigureAwait(false);
        if (changes is not null)
        {
            var changedFiles = string.Join(
                Environment.NewLine,
                changes.ChangedFiles.Select(file => $"- {file}"));

            throw new InvalidOperationException(
                $"""
                 Markdown files are not formatted.

                 Offending files:
                 {changedFiles}

                 Diff summary:
                 {changes.DiffStat}

                 Run FormatMarkdownModule locally and commit the changes.
                 """);
        }

        return None.Value;
    }
}
