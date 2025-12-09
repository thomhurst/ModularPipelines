using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "get-function-definition-version")]
public record AwsGreengrassGetFunctionDefinitionVersionOptions(
[property: CliOption("--function-definition-id")] string FunctionDefinitionId,
[property: CliOption("--function-definition-version-id")] string FunctionDefinitionVersionId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}