using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "authorize-data-share")]
public record AwsRedshiftAuthorizeDataShareOptions(
[property: CliOption("--data-share-arn")] string DataShareArn,
[property: CliOption("--consumer-identifier")] string ConsumerIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}