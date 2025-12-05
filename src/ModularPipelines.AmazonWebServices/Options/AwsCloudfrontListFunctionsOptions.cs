using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "list-functions")]
public record AwsCloudfrontListFunctionsOptions : AwsOptions
{
    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--stage")]
    public string? Stage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}