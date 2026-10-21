using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using YamlDotNet.Serialization.NamingConventions;

namespace ModularPipelines.GitHub;

internal abstract class GitHubPipelineFileWriter : IBuildSystemPipelineFileWriter
{
    public async Task WriteAsync(IPipelineContext pipelineHookContext)
    {
        var options = await GetGitHubPipelineFileWriterOptions(pipelineHookContext).ConfigureAwait(false);
        var maskEnvironment = options.SecretNamesToMask?
            .Select((name, index) => KeyValuePair.Create($"MODULARPIPELINES_MASK_VALUE_{index}", GetSecretExpression(name)))
            .ToDictionary() ?? [];
        var portableProjectPath = options.PipelineProjectPath.OriginalPath.Replace('\\', '/');
        var quotedProjectPath = $"'{portableProjectPath.Replace("'", "'\"'\"'", StringComparison.Ordinal)}'";

        var yaml = pipelineHookContext.Data.Yaml.ToYaml(new
        {
            options.Name,
            On = options.TriggerCondition,
            Jobs = new
            {
                Pipeline = new
                {
                    options.Environment,
                    RunsOn = options.Runner,

                    Steps = new object?[]
                    {
                        maskEnvironment.Count == 0 ? null : new
                        {
                          Name = "Mask Secret Values",
                          Shell = "bash",
                          Run = $$"""
                                for value in {{string.Join(" ", maskEnvironment.Keys.Select(key => $"\"${{{key}}}\""))}}; do
                                  value="${value//'%'/'%25'}"
                                  value="${value//$'\r'/'%0D'}"
                                  value="${value//$'\n'/'%0A'}"
                                  printf '::add-mask::%s\n' "$value"
                                done
                                """,
                          Env = maskEnvironment,
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
                            Shell = "bash",
                            Run =
                                $"dotnet run --project {quotedProjectPath} -c Release{(!string.IsNullOrWhiteSpace(options.DotNetRunFramework) ? $" --framework {options.DotNetRunFramework}" : string.Empty)}",
                            Env = options.EnvironmentVariables,
                        },
                    }.Where(x => x != null),
                },
            },
        }, HyphenatedNamingConvention.Instance);

        await options.OutputPath.WriteAsync(yaml).ConfigureAwait(false);
    }

    internal abstract Task<GitHubPipelineFileWriterOptions> GetGitHubPipelineFileWriterOptions(IPipelineContext pipelineHookContext);

    private static string GetSecretExpression(string name)
    {
        if (string.IsNullOrEmpty(name)
            || (!char.IsAsciiLetter(name[0]) && name[0] != '_')
            || name.Any(character => !char.IsAsciiLetterOrDigit(character) && character != '_'))
        {
            // Do not include the invalid input: it may be a secret value supplied by mistake.
            throw new ArgumentException(
                "Supply GitHub secret names, not values or expressions. Names must start with an ASCII letter or underscore and contain only ASCII letters, digits, or underscores.",
                nameof(GitHubPipelineFileWriterOptions.SecretNamesToMask));
        }

        return $"${{{{ secrets.{name} }}}}";
    }
}
