using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("new-relic", "plan", "list")]
public record AzNewRelicPlanListOptions : AzOptions
{
    [CliOption("--account-id")]
    public int? AccountId { get; set; }

    [CliOption("--organization-id")]
    public string? OrganizationId { get; set; }
}