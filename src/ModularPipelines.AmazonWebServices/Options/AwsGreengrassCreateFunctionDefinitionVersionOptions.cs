using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "create-function-definition-version")]
public record AwsGreengrassCreateFunctionDefinitionVersionOptions(
[property: CommandSwitch("--function-definition-id")] string FunctionDefinitionId
) : AwsOptions
{
    [CommandSwitch("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CommandSwitch("--default-config")]
    public string? DefaultConfig { get; set; }

    [CommandSwitch("--functions")]
    public string[]? Functions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}