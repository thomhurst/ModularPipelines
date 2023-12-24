using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billingconductor", "create-billing-group")]
public record AwsBillingconductorCreateBillingGroupOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--account-grouping")] string AccountGrouping,
[property: CommandSwitch("--computation-preference")] string ComputationPreference
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--primary-account-id")]
    public string? PrimaryAccountId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}