using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds-data", "commit-transaction")]
public record AwsRdsDataCommitTransactionOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--secret-arn")] string SecretArn,
[property: CliOption("--transaction-id")] string TransactionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}