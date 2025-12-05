using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "create")]
public record GcloudDataprocWorkflowTemplatesCreateOptions(
[property: CliArgument] string Template,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--dag-timeout")]
    public string? DagTimeout { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}