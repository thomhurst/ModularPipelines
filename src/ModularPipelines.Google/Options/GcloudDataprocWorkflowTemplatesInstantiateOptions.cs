using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "instantiate")]
public record GcloudDataprocWorkflowTemplatesInstantiateOptions(
[property: CliArgument] string Template,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }
}