using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "execution-groups", "list")]
public record GcloudComputeTpusExecutionGroupsListOptions : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}