using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "delete-response-headers-policy")]
public record AwsCloudfrontDeleteResponseHeadersPolicyOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}