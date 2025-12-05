using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "remove-dag-timeout")]
public record GcloudDataprocWorkflowTemplatesRemoveDagTimeoutOptions(
[property: CliArgument] string Template,
[property: CliArgument] string Region
) : GcloudOptions;