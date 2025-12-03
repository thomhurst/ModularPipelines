using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "workflow-templates", "get-iam-policy")]
public record GcloudDataprocWorkflowTemplatesGetIamPolicyOptions(
[property: CliArgument] string Template,
[property: CliArgument] string Region
) : GcloudOptions;