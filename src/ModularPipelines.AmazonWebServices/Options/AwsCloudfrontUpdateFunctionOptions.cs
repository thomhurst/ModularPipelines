using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "update-function")]
public record AwsCloudfrontUpdateFunctionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--if-match")] string IfMatch,
[property: CliOption("--function-config")] string FunctionConfig,
[property: CliOption("--function-code")] string FunctionCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}