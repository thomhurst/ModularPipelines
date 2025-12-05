using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "add-permission")]
public record AwsSnsAddPermissionOptions(
[property: CliOption("--topic-arn")] string TopicArn,
[property: CliOption("--label")] string Label,
[property: CliOption("--aws-account-id")] string[] AwsAccountId,
[property: CliOption("--action-name")] string[] ActionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}