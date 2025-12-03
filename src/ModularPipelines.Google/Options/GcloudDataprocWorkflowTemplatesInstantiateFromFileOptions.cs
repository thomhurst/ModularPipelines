using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "instantiate-from-file")]
public record GcloudDataprocWorkflowTemplatesInstantiateFromFileOptions(
[property: CliOption("--file")] string File
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}