using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "instances", "set-iam-policy")]
public record GcloudNotebooksInstancesSetIamPolicyOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;