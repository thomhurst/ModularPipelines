using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "delete")]
public record GcloudDataprocWorkflowTemplatesDeleteOptions(
[property: CliArgument] string Template,
[property: CliArgument] string Region
) : GcloudOptions;