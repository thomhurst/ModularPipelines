using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "ios", "run")]
public record GcloudFirebaseTestIosRunOptions(
[property: PositionalArgument] string Argspec
) : GcloudOptions
{
    [CommandSwitch("--app")]
    public string? App { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--client-details")]
    public IEnumerable<KeyValue>? ClientDetails { get; set; }

    [CommandSwitch("--num-flaky-test-attempts")]
    public string? NumFlakyTestAttempts { get; set; }

    [BooleanCommandSwitch("--record-video")]
    public bool? RecordVideo { get; set; }

    [CommandSwitch("--results-bucket")]
    public string? ResultsBucket { get; set; }

    [CommandSwitch("--results-dir")]
    public string? ResultsDir { get; set; }

    [CommandSwitch("--results-history-name")]
    public string? ResultsHistoryName { get; set; }

    [BooleanCommandSwitch("--test-special-entitlements")]
    public bool? TestSpecialEntitlements { get; set; }
}