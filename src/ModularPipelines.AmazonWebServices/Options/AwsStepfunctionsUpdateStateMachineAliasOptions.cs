using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "update-state-machine-alias")]
public record AwsStepfunctionsUpdateStateMachineAliasOptions(
[property: CommandSwitch("--state-machine-alias-arn")] string StateMachineAliasArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--routing-configuration")]
    public string[]? RoutingConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}