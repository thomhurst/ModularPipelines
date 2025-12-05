using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "test-function")]
public record AwsCloudfrontTestFunctionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--if-match")] string IfMatch,
[property: CliOption("--event-object")] string EventObject
) : AwsOptions
{
    [CliOption("--stage")]
    public string? Stage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}