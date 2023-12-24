using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "update-state-machine")]
public record AwsStepfunctionsUpdateStateMachineOptions(
[property: CommandSwitch("--state-machine-arn")] string StateMachineArn
) : AwsOptions
{
    [CommandSwitch("--definition")]
    public string? Definition { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--logging-configuration")]
    public string? LoggingConfiguration { get; set; }

    [CommandSwitch("--tracing-configuration")]
    public string? TracingConfiguration { get; set; }

    [CommandSwitch("--version-description")]
    public string? VersionDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}