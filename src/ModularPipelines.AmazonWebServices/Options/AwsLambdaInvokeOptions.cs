using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "invoke")]
public record AwsLambdaInvokeOptions(
[property: CommandSwitch("--function-name")] string FunctionName
) : AwsOptions
{
    [CommandSwitch("--invocation-type")]
    public string? InvocationType { get; set; }

    [CommandSwitch("--log-type")]
    public string? LogType { get; set; }

    [CommandSwitch("--client-context")]
    public string? ClientContext { get; set; }

    [CommandSwitch("--payload")]
    public string? Payload { get; set; }

    [CommandSwitch("--qualifier")]
    public string? Qualifier { get; set; }
}