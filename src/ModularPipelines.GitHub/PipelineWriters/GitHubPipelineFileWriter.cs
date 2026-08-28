using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using YamlDotNet.Serialization.NamingConventions;

namespace ModularPipelines.GitHub.PipelineWriters;

internal abstract class GitHubPipelineFileWriter : IBuildSystemPipelineFileWriter
{
    public async Task WriteAsync(IPipelineContext pipelineHookContext)
    {
        var options = await GetGitHubPipelineFileWriterOptions(pipelineHookContext);

        var yaml = pipelineHookContext.Data.Yaml.ToYaml(new
        {
            Name = options.Name,
            On = options.TriggerCondition,
            Jobs = new
            {
                Pipeline = new
                {
                    Environment = options.Environment,
                    RunsOn = options.Runner,

                    Steps = new object?[]
                    {
                        options.ValuesToMask?.Any() != true ? null : new
                        {
                          Name = "Mask Secret Values",
                          Run = string.Join(Environment.NewLine, options.ValuesToMask.Select(x => $"echo \"::add-mask::{x}")),
                        },
                        new
                        {
                            Name = "Checkout",
                            Uses = GitHubActionVersions.Checkout,
                            With = new
                            {
                                FetchDepth = 0,
                                PersistCredentials = false,
                            },
                        },
                        new
                        {
                            Name = "Setup .NET SDK",
                            Uses = GitHubActionVersions.SetupDotNet,
                            With = new
                            {
                              DotnetVersion = options.DotNetVersion,
                            },
                        },
                        !options.CacheNuGet ? null : new
                        {
                            Name = "Cache NuGet",
                            Uses = GitHubActionVersions.Cache,
                            With = new
                            {
                                Path = "~/.nuget/packages",
                                Key = "${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}",
                                RestoreKeys = "${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}",
                            },
                        },
                        new
                        {
                            Name = "Run Pipeline",
                            Run =
                                $"dotnet run --project {options.PipelineProjectPath.OriginalPath} -c Release{(!string.IsNullOrWhiteSpace(options.DotNetRunFramework) ? $" --framework {options.DotNetRunFramework}" : string.Empty)}",
                            Env = options.EnvironmentVariables,
                        },
                    }.Where(x => x != null),
                },
            },
        }, HyphenatedNamingConvention.Instance);

        await options.OutputPath.WriteAsync(yaml);
    }

    internal abstract Task<GitHubPipelineFileWriterOptions> GetGitHubPipelineFileWriterOptions(IPipelineContext pipelineHookContext);
}
