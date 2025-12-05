using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billingconductor", "list-account-associations")]
public record AwsBillingconductorListAccountAssociationsOptions : AwsOptions
{
    [CliOption("--billing-period")]
    public string? BillingPeriod { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}