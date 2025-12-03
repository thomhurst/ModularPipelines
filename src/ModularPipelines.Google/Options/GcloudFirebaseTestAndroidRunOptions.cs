using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "android", "run")]
public record GcloudFirebaseTestAndroidRunOptions(
[property: CliArgument] string Argspec
) : GcloudOptions
{
    [CliOption("--additional-apks")]
    public string[]? AdditionalApks { get; set; }

    [CliOption("--app-package")]
    public string? AppPackage { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--auto-google-login")]
    public bool? AutoGoogleLogin { get; set; }

    [CliOption("--client-details")]
    public IEnumerable<KeyValue>? ClientDetails { get; set; }

    [CliOption("--directories-to-pull")]
    public string[]? DirectoriesToPull { get; set; }

    [CliOption("--environment-variables")]
    public IEnumerable<KeyValue>? GcloudFEnvironmentVariables { get; set; }

    [CliOption("--network-profile")]
    public string? NetworkProfile { get; set; }

    [CliOption("--num-flaky-test-attempts")]
    public string? NumFlakyTestAttempts { get; set; }

    [CliOption("--obb-files")]
    public string[]? ObbFiles { get; set; }

    [CliOption("--other-files")]
    public string[]? OtherFiles { get; set; }

    [CliFlag("--performance-metrics")]
    public bool? PerformanceMetrics { get; set; }

    [CliFlag("--record-video")]
    public bool? RecordVideo { get; set; }

    [CliOption("--results-bucket")]
    public string? ResultsBucket { get; set; }

    [CliOption("--results-dir")]
    public string? ResultsDir { get; set; }

    [CliOption("--results-history-name")]
    public string? ResultsHistoryName { get; set; }
}