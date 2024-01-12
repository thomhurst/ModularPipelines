using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "workflow-templates", "set-dag-timeout")]
public record GcloudDataprocWorkflowTemplatesSetDagTimeoutOptions(
[property: PositionalArgument] string Template,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--dag-timeout")] string DagTimeout
) : GcloudOptions;