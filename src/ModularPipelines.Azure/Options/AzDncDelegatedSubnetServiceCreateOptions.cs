using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dnc", "delegated-subnet-service", "create")]
public record AzDncDelegatedSubnetServiceCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--allocation-block-prefix-size")]
    public string? AllocationBlockPrefixSize { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--subnet-details-id")]
    public string? SubnetDetailsId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}