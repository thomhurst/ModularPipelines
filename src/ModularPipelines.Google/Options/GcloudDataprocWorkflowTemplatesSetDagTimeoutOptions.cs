using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "set-dag-timeout")]
public record GcloudDataprocWorkflowTemplatesSetDagTimeoutOptions(
[property: CliArgument] string Template,
[property: CliArgument] string Region,
[property: CliOption("--dag-timeout")] string DagTimeout
) : GcloudOptions;