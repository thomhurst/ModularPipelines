using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "execution-groups", "describe")]
public record GcloudComputeTpusExecutionGroupsDescribeOptions(
[property: PositionalArgument] string ExecutionGroupName
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}