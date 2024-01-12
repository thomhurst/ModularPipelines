using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "android", "run")]
public record GcloudFirebaseTestAndroidRunOptions(
[property: PositionalArgument] string Argspec
) : GcloudOptions
{
    [CommandSwitch("--additional-apks")]
    public string[]? AdditionalApks { get; set; }

    [CommandSwitch("--app-package")]
    public string? AppPackage { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--auto-google-login")]
    public bool? AutoGoogleLogin { get; set; }

    [CommandSwitch("--client-details")]
    public IEnumerable<KeyValue>? ClientDetails { get; set; }

    [CommandSwitch("--directories-to-pull")]
    public string[]? DirectoriesToPull { get; set; }

    [CommandSwitch("--environment-variables")]
    public IEnumerable<KeyValue>? GcloudFEnvironmentVariables { get; set; }

    [CommandSwitch("--network-profile")]
    public string? NetworkProfile { get; set; }

    [CommandSwitch("--num-flaky-test-attempts")]
    public string? NumFlakyTestAttempts { get; set; }

    [CommandSwitch("--obb-files")]
    public string[]? ObbFiles { get; set; }

    [CommandSwitch("--other-files")]
    public string[]? OtherFiles { get; set; }

    [BooleanCommandSwitch("--performance-metrics")]
    public bool? PerformanceMetrics { get; set; }

    [BooleanCommandSwitch("--record-video")]
    public bool? RecordVideo { get; set; }

    [CommandSwitch("--results-bucket")]
    public string? ResultsBucket { get; set; }

    [CommandSwitch("--results-dir")]
    public string? ResultsDir { get; set; }

    [CommandSwitch("--results-history-name")]
    public string? ResultsHistoryName { get; set; }
}