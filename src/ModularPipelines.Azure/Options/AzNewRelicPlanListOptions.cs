using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("new-relic", "plan", "list")]
public record AzNewRelicPlanListOptions : AzOptions
{
    [CommandSwitch("--account-id")]
    public int? AccountId { get; set; }

    [CommandSwitch("--organization-id")]
    public string? OrganizationId { get; set; }
}