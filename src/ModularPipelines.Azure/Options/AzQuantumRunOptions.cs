using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quantum", "run")]
public record AzQuantumRunOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--target-id")] string TargetId,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--entry-point")]
    public string? EntryPoint { get; set; }

    [CommandSwitch("--job-input-file")]
    public string? JobInputFile { get; set; }

    [CommandSwitch("--job-input-format")]
    public string? JobInputFormat { get; set; }

    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--job-output-format")]
    public string? JobOutputFormat { get; set; }

    [CommandSwitch("--job-params")]
    public string? JobParams { get; set; }

    [BooleanCommandSwitch("--no-build")]
    public bool? NoBuild { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--shots")]
    public string? Shots { get; set; }

    [CommandSwitch("--storage")]
    public string? Storage { get; set; }

    [CommandSwitch("--target-capability")]
    public string? TargetCapability { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ProgramArgs { get; set; }
}