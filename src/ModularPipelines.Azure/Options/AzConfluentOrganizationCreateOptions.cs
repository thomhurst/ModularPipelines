using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("confluent", "organization", "create")]
public record AzConfluentOrganizationCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--plan-id")] string PlanId,
[property: CommandSwitch("--plan-name")] string PlanName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--term-unit")] string TermUnit
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--offer-id")]
    public string? OfferId { get; set; }

    [CommandSwitch("--publisher-id")]
    public string? PublisherId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}