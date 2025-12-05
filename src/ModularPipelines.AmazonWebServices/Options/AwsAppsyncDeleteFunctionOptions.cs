using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "delete-function")]
public record AwsAppsyncDeleteFunctionOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--function-id")] string FunctionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}