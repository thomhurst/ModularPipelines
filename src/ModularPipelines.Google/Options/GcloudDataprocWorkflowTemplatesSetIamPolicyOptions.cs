using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "set-iam-policy")]
public record GcloudDataprocWorkflowTemplatesSetIamPolicyOptions(
[property: CliArgument] string Template,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions;