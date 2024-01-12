using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workbench", "instances", "rollback")]
public record GcloudWorkbenchInstancesRollbackOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--target-snapshot")] string TargetSnapshot
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}