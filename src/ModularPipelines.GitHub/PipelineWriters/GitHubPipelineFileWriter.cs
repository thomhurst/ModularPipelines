using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using YamlDotNet.Serialization.NamingConventions;

namespace ModularPipelines.GitHub.PipelineWriters;

public abstract class GitHubPipelineFileWriter : IBuildSystemPipelineFileWriter
{
    public async Task Write(IPipelineHookContext pipelineHookContext)
    {
        var options = await GetGitHubPipelineFileWriterOptions(pipelineHookContext);
        
        var yaml = pipelineHookContext.Yaml.ToYaml(new
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
                            Uses = "actions/checkout@v3",
                            With = new
                            {
                                FetchDepth = 0,
                                PersistCredentials = false,
                            },
                        },
                        new
                        {
                            Name = "Setup .NET SDK",
                            Uses = "actions/setup-dotnet@v3",
                            With = new
                            {
                              DotnetVersion = "8.0.x",
                            },
                        },
                        !options.CacheNuGet ? null : new
                        {
                            Name = "Cache NuGet",
                            Uses = "actions/cache@v3",
                            With = new
                            {
                                path= "~/.nuget/packages",
                                key= "${{ runner.os }}-nuget-${{ hashFiles('**/packages.lock.json') }}",
                                RestoreKeys = "${{ runner.os }}-nuget-",
                            },
                        },
                        new
                        {
                            Name = "Run Pipeline",
                            Run =
                                $"dotnet run -c Release {(!string.IsNullOrWhiteSpace(options.DotNetRunFramework) ? $"--framework {options.DotNetRunFramework} " : string.Empty)}{options.PipelineProjectPath}",
                            Env = options.EnvironmentVariables,
                        },
                    }.Where(x => x != null),
                },
            },
        }, HyphenatedNamingConvention.Instance);

        await options.OutputPath.WriteAsync(yaml);
    }

    public abstract Task<GitHubPipelineFileWriterOptions> GetGitHubPipelineFileWriterOptions(IPipelineHookContext pipelineHookContext);
}