using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billingconductor", "disassociate-accounts")]
public record AwsBillingconductorDisassociateAccountsOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--account-ids")] string[] AccountIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}