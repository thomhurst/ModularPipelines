using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "extend-transaction")]
public record AwsLakeformationExtendTransactionOptions : AwsOptions
{
    [CliOption("--transaction-id")]
    public string? TransactionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}