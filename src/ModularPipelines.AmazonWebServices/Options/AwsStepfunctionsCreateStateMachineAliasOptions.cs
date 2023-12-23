using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "create-state-machine-alias")]
public record AwsStepfunctionsCreateStateMachineAliasOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--routing-configuration")] string[] RoutingConfiguration
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}