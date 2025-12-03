using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "describe-function")]
public record AwsCloudfrontDescribeFunctionOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--stage")]
    public string? Stage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}