using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using YamlDotNet.Serialization.NamingConventions;

namespace ModularPipelines.GitHub.PipelineWriters;

internal sealed class DistributedGitHubPipelineFileWriter : IBuildSystemPipelineFileWriter
{
    private const string ValidateRetryScopeCommand = """
        if [ "${{ needs.initialize.outputs.run-identifier }}" != "${GITHUB_RUN_ID}-${GITHUB_RUN_ATTEMPT}" ]; then
          echo "::error::Distributed workflows require 'Re-run all jobs'; partial retries cannot recreate the worker matrix."
          exit 1
        fi
        """;

    // Ordered by preference; the matrix provisions runners in this order.
    // Beyond this many planning-safe conditions per module, runner planning stops enumerating outcomes.
    private const int MaximumEnumeratedConditions = 10;

    private static readonly (Capability OperatingSystem, string Runner)[] Runners =
    [
        (Capability.Linux, "ubuntu-latest"),
        (Capability.Windows, "windows-latest"),
        (Capability.MacOS, "macos-latest"),
    ];

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
            ["MODULARPIPELINES_INSTANCE_INDEX"] = "${{ matrix.instance }}",
            ["MODULARPIPELINES_TOTAL_INSTANCES"] = matrix.Count.ToString(System.Globalization.CultureInfo.InvariantCulture),
            ["REDIS_URL"] = $"${{{{ secrets.{_options.RedisSecretName} }}}}",
            ["MODULARPIPELINES_RUN_ID"] = "${{ needs.initialize.outputs.run-identifier }}",
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
            .SelectMany(module => GetRequiredOperatingSystems(module.GetType()))
            .ToHashSet();

        var runners = Runners
            .Where(runner => requiredOperatingSystems.Contains(runner.OperatingSystem))
            .Select(static runner => runner.Runner)
            .Concat(Enumerable.Repeat(_options.DefaultRunner, _options.ExtraWorkers));

        return new[] { _options.DefaultRunner }
            .Concat(runners)
            .Select((runner, index) => new MatrixEntry(index, runner))
            .ToArray();
    }

    private static IEnumerable<Capability> GetRequiredOperatingSystems(Type moduleType)
    {
        var formula = ConditionFormula.ForModule(
            moduleType.GetCustomAttributes(inherit: true).OfType<IConditionAttribute>());
        var operatingSystems = new HashSet<Capability>();
        foreach (var conditionValue in GetPossibleConditionValues(formula))
        {
            // Each outcome of the master's planning-safe conditions is a requirement the master may stamp.
            if (CapabilityConditions.Combine(moduleType, conditionValue) is not { } requirement
                || GetAllowedOperatingSystems(requirement.Clauses) is not { } allowedOperatingSystems)
            {
                // Impossible outcomes are skipped, and unrestricted ones run on the default runner.
                continue;
            }

            EnsureSupported(allowedOperatingSystems);
            operatingSystems.UnionWith(allowedOperatingSystems);
        }

        return Runners
            .Select(static runner => runner.OperatingSystem)
            .Where(operatingSystems.Contains);
    }

    /// <summary>
    /// Returns the formula's value for every combination of its planning-safe conditions, which the
    /// master evaluates at run time. Worker-only conditions stay unconstrained.
    /// </summary>
    private static IEnumerable<FormulaValue> GetPossibleConditionValues(ConditionFormula? formula)
    {
        if (formula is null)
        {
            yield return FormulaValue.True;
            yield break;
        }

        var planningAtoms = formula.Atoms
            .Where(static atom => atom.IsPlanning)
            .Distinct()
            .ToArray();
        if (planningAtoms.Length > MaximumEnumeratedConditions)
        {
            // Too many combinations: plan for what the conditions need whatever they return.
            yield return formula.Evaluate(static _ => true);
            yield break;
        }

        for (var combination = 0; combination < 1 << planningAtoms.Length; combination++)
        {
            var values = planningAtoms
                .Select((atom, index) => (atom, value: (combination & (1 << index)) != 0))
                .ToDictionary(static pair => pair.atom, static pair => pair.value);
            yield return formula.Evaluate(atom => !values.TryGetValue(atom, out var value) || value);
        }
    }

    /// <summary>
    /// Returns the operating systems allowed by clauses that list only operating systems, or
    /// <c>null</c> when no clause restricts the operating system.
    /// </summary>
    private static HashSet<Capability>? GetAllowedOperatingSystems(IEnumerable<IReadOnlyList<Capability>> clauses)
    {
        HashSet<Capability>? allowedOperatingSystems = null;
        foreach (var clause in clauses.Where(static clause =>
                     clause.All(static capability => capability.IsOperatingSystem)))
        {
            if (allowedOperatingSystems is null)
            {
                allowedOperatingSystems = [.. clause];
            }
            else
            {
                allowedOperatingSystems.IntersectWith(clause);
            }
        }

        return allowedOperatingSystems;
    }

    private static void EnsureSupported(IReadOnlyCollection<Capability> operatingSystems)
    {
        if (!Runners.Any(runner => operatingSystems.Contains(runner.OperatingSystem)))
        {
            throw new InvalidOperationException(
                "Distributed GitHub workflows do not support the required operating-system capability " +
                $"'{string.Join(" | ", operatingSystems)}'.");
        }
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
