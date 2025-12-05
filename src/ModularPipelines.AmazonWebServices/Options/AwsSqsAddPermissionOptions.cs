using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "add-permission")]
public record AwsSqsAddPermissionOptions(
[property: CliOption("--queue-url")] string QueueUrl,
[property: CliOption("--label")] string Label,
[property: CliOption("--aws-account-ids")] string[] AwsAccountIds,
[property: CliOption("--actions")] string[] Actions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}