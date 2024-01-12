using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "workflow-templates", "remove-dag-timeout")]
public record GcloudDataprocWorkflowTemplatesRemoveDagTimeoutOptions(
[property: PositionalArgument] string Template,
[property: PositionalArgument] string Region
) : GcloudOptions;