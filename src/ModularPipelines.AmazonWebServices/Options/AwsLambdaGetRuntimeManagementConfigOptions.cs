using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "get-runtime-management-config")]
public record AwsLambdaGetRuntimeManagementConfigOptions(
[property: CommandSwitch("--function-name")] string FunctionName
) : AwsOptions
{
    [CommandSwitch("--qualifier")]
    public string? Qualifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}