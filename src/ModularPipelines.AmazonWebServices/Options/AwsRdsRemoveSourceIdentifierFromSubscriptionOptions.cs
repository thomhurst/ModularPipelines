using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "remove-source-identifier-from-subscription")]
public record AwsRdsRemoveSourceIdentifierFromSubscriptionOptions(
[property: CliOption("--subscription-name")] string SubscriptionName,
[property: CliOption("--source-identifier")] string SourceIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}