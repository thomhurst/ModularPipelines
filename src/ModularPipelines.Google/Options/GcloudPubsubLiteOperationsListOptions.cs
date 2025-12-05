using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-operations", "list")]
public record GcloudPubsubLiteOperationsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliOption("--done")]
    public string? Done { get; set; }

    [CliOption("--subscription")]
    public string? Subscription { get; set; }
}