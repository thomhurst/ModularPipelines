using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "execution-groups", "describe")]
public record GcloudComputeTpusExecutionGroupsDescribeOptions(
[property: CliArgument] string ExecutionGroupName
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}