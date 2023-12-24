using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "put-runtime-management-config")]
public record AwsLambdaPutRuntimeManagementConfigOptions(
[property: CommandSwitch("--function-name")] string FunctionName,
[property: CommandSwitch("--update-runtime-on")] string UpdateRuntimeOn
) : AwsOptions
{
    [CommandSwitch("--qualifier")]
    public string? Qualifier { get; set; }

    [CommandSwitch("--runtime-version-arn")]
    public string? RuntimeVersionArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}