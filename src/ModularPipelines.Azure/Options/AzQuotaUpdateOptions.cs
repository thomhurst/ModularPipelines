using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("quota", "update")]
public record AzQuotaUpdateOptions(
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--scope")] string Scope
) : AzOptions
{
    [CliOption("--limit-object")]
    public string? LimitObject { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }
}