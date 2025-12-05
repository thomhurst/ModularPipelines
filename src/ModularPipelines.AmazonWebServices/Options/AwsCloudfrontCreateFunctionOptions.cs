using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-function")]
public record AwsCloudfrontCreateFunctionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--function-config")] string FunctionConfig,
[property: CliOption("--function-code")] string FunctionCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}