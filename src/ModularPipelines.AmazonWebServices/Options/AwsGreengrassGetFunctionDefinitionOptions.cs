using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "get-function-definition")]
public record AwsGreengrassGetFunctionDefinitionOptions(
[property: CliOption("--function-definition-id")] string FunctionDefinitionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}