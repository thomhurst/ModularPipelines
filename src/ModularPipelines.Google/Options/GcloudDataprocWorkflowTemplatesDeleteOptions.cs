using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "workflow-templates", "delete")]
public record GcloudDataprocWorkflowTemplatesDeleteOptions(
[property: PositionalArgument] string Template,
[property: PositionalArgument] string Region
) : GcloudOptions;