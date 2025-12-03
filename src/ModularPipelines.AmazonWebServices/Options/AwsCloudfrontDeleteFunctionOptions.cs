using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "delete-function")]
public record AwsCloudfrontDeleteFunctionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--if-match")] string IfMatch
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}