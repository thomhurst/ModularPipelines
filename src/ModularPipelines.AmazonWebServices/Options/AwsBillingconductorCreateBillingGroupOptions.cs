using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billingconductor", "create-billing-group")]
public record AwsBillingconductorCreateBillingGroupOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--account-grouping")] string AccountGrouping,
[property: CliOption("--computation-preference")] string ComputationPreference
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--primary-account-id")]
    public string? PrimaryAccountId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}