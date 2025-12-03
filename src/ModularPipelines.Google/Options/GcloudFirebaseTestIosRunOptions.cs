using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "ios", "run")]
public record GcloudFirebaseTestIosRunOptions(
[property: CliArgument] string Argspec
) : GcloudOptions
{
    [CliOption("--app")]
    public string? App { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--client-details")]
    public IEnumerable<KeyValue>? ClientDetails { get; set; }

    [CliOption("--num-flaky-test-attempts")]
    public string? NumFlakyTestAttempts { get; set; }

    [CliFlag("--record-video")]
    public bool? RecordVideo { get; set; }

    [CliOption("--results-bucket")]
    public string? ResultsBucket { get; set; }

    [CliOption("--results-dir")]
    public string? ResultsDir { get; set; }

    [CliOption("--results-history-name")]
    public string? ResultsHistoryName { get; set; }

    [CliFlag("--test-special-entitlements")]
    public bool? TestSpecialEntitlements { get; set; }
}