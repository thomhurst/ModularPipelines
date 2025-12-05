using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "deauthorize-data-share")]
public record AwsRedshiftDeauthorizeDataShareOptions(
[property: CliOption("--data-share-arn")] string DataShareArn,
[property: CliOption("--consumer-identifier")] string ConsumerIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}