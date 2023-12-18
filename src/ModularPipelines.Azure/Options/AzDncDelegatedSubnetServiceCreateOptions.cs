using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dnc", "delegated-subnet-service", "create")]
public record AzDncDelegatedSubnetServiceCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--allocation-block-prefix-size")]
    public string? AllocationBlockPrefixSize { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--subnet-details-id")]
    public string? SubnetDetailsId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}