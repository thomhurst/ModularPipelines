using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "remove-permission")]
public record AwsSnsRemovePermissionOptions(
[property: CliOption("--topic-arn")] string TopicArn,
[property: CliOption("--label")] string Label
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}