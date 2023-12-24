using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "create-function-url-config")]
public record AwsLambdaCreateFunctionUrlConfigOptions(
[property: CommandSwitch("--function-name")] string FunctionName,
[property: CommandSwitch("--auth-type")] string AuthType
) : AwsOptions
{
    [CommandSwitch("--qualifier")]
    public string? Qualifier { get; set; }

    [CommandSwitch("--cors")]
    public string? Cors { get; set; }

    [CommandSwitch("--invoke-mode")]
    public string? InvokeMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}