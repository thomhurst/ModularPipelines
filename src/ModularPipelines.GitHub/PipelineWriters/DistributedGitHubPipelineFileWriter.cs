using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using YamlDotNet.Serialization.NamingConventions;

namespace ModularPipelines.GitHub.PipelineWriters;

internal sealed class DistributedGitHubPipelineFileWriter : IBuildSystemPipelineFileWriter
{
    private const string Linux = "linux";
    private const string Windows = "windows";
    private const string MacOS = "macos";
    private const string AlternativeOperatingSystemPrefix = "operating-system:";

    private static readonly IReadOnlyDictionary<string, string> RunnerByOperatingSystem =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [Linux] = "ubuntu-latest",
            [Windows] = "windows-latest",
            [MacOS] = "macos-latest",
        };

    private static readonly string[] OperatingSystemOrder = [Linux, Windows, MacOS];

    private readonly DistributedWorkflowOptions _options;
    private readonly IReadOnlyList<IModule> _modules;

    internal DistributedGitHubPipelineFileWriter(
        DistributedWorkflowOptions options,
        IEnumerable<IModule> modules)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(modules);

        _options = options;
        _modules = modules.ToArray();
    }

    public async Task Write(IPipelineContext pipelineHookContext)
    {
        ValidateOptions();

        var matrix = BuildMatrix();
        var environmentVariables = new Dictionary<string, string>(
            _options.EnvironmentVariables ?? new Dictionary<string, string>(),
            StringComparer.Ordinal)
        {
            ["INSTANCE_INDEX"] = "${{ matrix.instance }}",
            ["TOTAL_INSTANCES"] = matrix.Count.ToString(System.Globalization.CultureInfo.InvariantCulture),
            ["REDIS_URL"] = $"${{{{ secrets.{_options.RedisSecretName} }}}}",
        };

        var yaml = pipelineHookContext.Data.Yaml.ToYaml(new
        {
            Name = _options.Name,
            On = _options.TriggerCondition,
            Jobs = new
            {
                Pipeline = new
                {
                    Strategy = new
                    {
                        FailFast = false,
                        Matrix = new
                        {
                            Include = matrix,
                        },
                    },
                    RunsOn = "${{ matrix.os }}",
                    Steps = new object?[]
                    {
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
                                DotnetVersion = _options.DotNetVersion,
                            },
                        },
                        !_options.CacheNuGet ? null : new
                        {
                            Name = "Cache NuGet",
                            Uses = GitHubActionVersions.Cache,
                            With = new
                            {
                                Path = "~/.nuget/packages",
                                Key = "${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}",
                                RestoreKeys = "${{ runner.os }}-nuget-",
                            },
                        },
                        new
                        {
                            Name = "Run Pipeline",
                            Run = BuildRunCommand(),
                            Env = environmentVariables,
                        },
                    }.Where(step => step is not null),
                },
            },
        }, HyphenatedNamingConvention.Instance);

        _options.OutputPath.Folder?.Create();
        await _options.OutputPath.WriteAsync(yaml).ConfigureAwait(false);
    }

    private IReadOnlyList<MatrixEntry> BuildMatrix()
    {
        var requiredOperatingSystems = _modules
            .SelectMany(module => module.GetType().GetCustomAttributes<RequiresCapabilityAttribute>(inherit: true))
            .SelectMany(attribute => ParseOperatingSystems(attribute.Capability))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var runners = OperatingSystemOrder
            .Where(requiredOperatingSystems.Contains)
            .Select(operatingSystem => RunnerByOperatingSystem[operatingSystem])
            .Concat(Enumerable.Repeat(_options.DefaultRunner, _options.ExtraWorkers));

        return new[] { _options.DefaultRunner }
            .Concat(runners)
            .Select((runner, index) => new MatrixEntry(index, runner))
            .ToArray();
    }

    private static IEnumerable<string> ParseOperatingSystems(string capability)
    {
        if (RunnerByOperatingSystem.ContainsKey(capability))
        {
            return [capability];
        }

        if (!capability.StartsWith(AlternativeOperatingSystemPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return [];
        }

        return capability[AlternativeOperatingSystemPrefix.Length..]
            .Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(RunnerByOperatingSystem.ContainsKey);
    }

    private string BuildRunCommand()
    {
        var framework = string.IsNullOrWhiteSpace(_options.DotNetRunFramework)
            ? string.Empty
            : $" --framework {_options.DotNetRunFramework}";

        return $"dotnet run --project {_options.PipelineProjectPath.OriginalPath} -c Release{framework}";
    }

    private void ValidateOptions()
    {
        if (_options.Backend != DistributedBackend.Redis)
        {
            throw new NotSupportedException($"Distributed backend '{_options.Backend}' is not supported.");
        }

        ArgumentOutOfRangeException.ThrowIfNegative(_options.ExtraWorkers);
        ArgumentException.ThrowIfNullOrWhiteSpace(_options.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(_options.DotNetVersion);
        ArgumentException.ThrowIfNullOrWhiteSpace(_options.DefaultRunner);
        ArgumentException.ThrowIfNullOrWhiteSpace(_options.RedisSecretName);
        ArgumentNullException.ThrowIfNull(_options.OutputPath);
        ArgumentNullException.ThrowIfNull(_options.PipelineProjectPath);
        ArgumentNullException.ThrowIfNull(_options.TriggerCondition);
    }

    private sealed record MatrixEntry(int Instance, string Os);
}
