using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snapshot", "grant-access")]
public record AzSnapshotGrantAccessOptions(
[property: CliOption("--duration-in-seconds")] string DurationInSeconds
) : AzOptions
{
    [CliOption("--access-level")]
    public string? AccessLevel { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}