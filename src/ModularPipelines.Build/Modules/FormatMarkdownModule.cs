using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Node.Extensions;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Build.Modules;

[RunIfAll<ModularPipelines.Conditions.OnLinux>]
[DependsOn<GenerateReadMeModule>]
public class FormatMarkdownModule : Module<CommandResult>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(ctx =>
        {
            if (ctx.GitHub().EnvironmentVariables.EventName != "pull_request")
            {
                return SkipDecision.Skip("Not a pull request");
            }

            return SkipDecision.DoNotSkip;
        })
        .WithAlwaysRun()
        .Build();

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await context.Node().Npm.Install(new NpmInstallOptions
        {
            Arguments =
            [
                "remark-cli",
                "remark-preset-lint-consistent",
                "remark-preset-lint-recommended",
                "remark-lint-list-item-indent"
            ],
            SaveDev = true,
        }, cancellationToken);

        var repositoryInfo = await context.Git().Information.GetInfoAsync().ConfigureAwait(false)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");
        var filesToFormat = new List<string>
        {
            repositoryInfo.Root.FindFile(x => x.Name == "README.md")!.Path,
            repositoryInfo.Root.FindFile(x => x.Name == "README_Template.md")!.Path,
        };

        foreach (var fileToFormat in filesToFormat)
        {
            await context.Node().Npx.ExecuteAsync(new NpxOptions
            {
                Arguments =
                [
                    "remark",
                    fileToFormat,
                    "--use", "remark-lint",
                    "--use", "remark-preset-lint-consistent",
                    "--use", "remark-preset-lint-recommended",
                    "--output"
                ],
            }, cancellationToken);
        }

        if (await GitHelpers.HasUncommittedChanges(context, filesToFormat))
        {
            throw new InvalidOperationException(
                "Markdown files are not formatted. Run FormatMarkdownModule locally and commit the changes.");
        }

        return null;
    }
}
