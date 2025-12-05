using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sqs", "remove-permission")]
public record AwsSqsRemovePermissionOptions(
[property: CliOption("--queue-url")] string QueueUrl,
[property: CliOption("--label")] string Label
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}