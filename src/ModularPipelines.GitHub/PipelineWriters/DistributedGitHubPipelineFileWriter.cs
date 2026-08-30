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
    private const string ValidateRetryScopeCommand = """
        if [ "${{ needs.initialize.outputs.run-identifier }}" != "${GITHUB_RUN_ID}-${GITHUB_RUN_ATTEMPT}" ]; then
          echo "::error::Distributed workflows require 'Re-run all jobs'; partial retries cannot recreate the worker matrix."
          exit 1
        fi
        """;

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

    public async Task WriteAsync(IPipelineContext pipelineHookContext)
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
            ["RUN_IDENTIFIER"] = "${{ needs.initialize.outputs.run-identifier }}",
        };

        var yaml = pipelineHookContext.Data.Yaml.ToYaml(new
        {
            Name = _options.Name,
            On = _options.TriggerCondition,
            Jobs = new
            {
                Initialize = new
                {
                    RunsOn = "ubuntu-latest",
                    Outputs = new Dictionary<string, string>
                    {
                        ["run-identifier"] = "${{ steps.identifier.outputs.value }}",
                    },
                    Steps = new[]
                    {
                        new
                        {
                            Name = "Initialize coordination",
                            Id = "identifier",
                            Shell = "bash",
                            Run = "echo \"value=${GITHUB_RUN_ID}-${GITHUB_RUN_ATTEMPT}\" >> \"$GITHUB_OUTPUT\"",
                        },
                    },
                },
                Pipeline = new
                {
                    Needs = "initialize",
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
                            Name = "Validate retry scope",
                            Shell = "bash",
                            Run = ValidateRetryScopeCommand,
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
                            Shell = "bash",
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
            .SelectMany(module => GetRequiredCapabilities(module.GetType()))
            .SelectMany(ParseOperatingSystems)
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

    private static IEnumerable<string> GetRequiredCapabilities(Type moduleType)
    {
        var declaredCapabilities = moduleType
            .GetCustomAttributes<RequiresCapabilityAttribute>(inherit: true)
            .Select(attribute => attribute.Capability);
        var operatingSystemConditions = moduleType
            .GetCustomAttributes(inherit: true)
            .OfType<IConditionAttribute>()
            .SelectMany(OperatingSystemConditions.GetTargets);

        return declaredCapabilities.Concat(operatingSystemConditions);
    }

    private static IEnumerable<string> ParseOperatingSystems(string capability)
    {
        if (!OperatingSystemConditions.TryGetCapabilityRoute(capability, out var route))
        {
            return [];
        }

        var supportedOperatingSystems = route.OperatingSystems
            .Where(RunnerByOperatingSystem.ContainsKey)
            .ToArray();
        if (supportedOperatingSystems.Length == 0)
        {
            throw new InvalidOperationException(
                $"Distributed GitHub workflows do not support the required operating-system capability '{capability}'.");
        }

        return supportedOperatingSystems;
    }

    private string BuildRunCommand()
    {
        var framework = string.IsNullOrWhiteSpace(_options.DotNetRunFramework)
            ? string.Empty
            : $" --framework {_options.DotNetRunFramework}";

        var portableProjectPath = _options.PipelineProjectPath.OriginalPath.Replace('\\', '/');
        var quotedProjectPath = QuotePosixShellArgument(portableProjectPath);
        return $"dotnet run --project {quotedProjectPath} -c Release{framework}";
    }

    private static string QuotePosixShellArgument(string value) =>
        $"'{value.Replace("'", "'\"'\"'", StringComparison.Ordinal)}'";

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
