using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "get-function-definition-version")]
public record AwsGreengrassGetFunctionDefinitionVersionOptions(
[property: CommandSwitch("--function-definition-id")] string FunctionDefinitionId,
[property: CommandSwitch("--function-definition-version-id")] string FunctionDefinitionVersionId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}