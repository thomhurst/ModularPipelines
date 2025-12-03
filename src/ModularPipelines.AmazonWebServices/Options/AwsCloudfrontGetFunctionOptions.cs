using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "get-function")]
public record AwsCloudfrontGetFunctionOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--stage")]
    public string? Stage { get; set; }
}