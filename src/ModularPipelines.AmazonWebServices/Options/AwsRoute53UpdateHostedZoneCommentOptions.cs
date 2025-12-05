using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "update-hosted-zone-comment")]
public record AwsRoute53UpdateHostedZoneCommentOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}