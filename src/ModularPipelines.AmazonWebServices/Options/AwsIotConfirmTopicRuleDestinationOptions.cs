using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "confirm-topic-rule-destination")]
public record AwsIotConfirmTopicRuleDestinationOptions(
[property: CliOption("--confirmation-token")] string ConfirmationToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}