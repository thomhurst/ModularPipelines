using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds-data", "begin-transaction")]
public record AwsRdsDataBeginTransactionOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--secret-arn")] string SecretArn
) : AwsOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }

    [CliOption("--schema")]
    public string? Schema { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}