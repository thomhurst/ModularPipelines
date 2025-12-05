using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workbench", "instances", "set-iam-policy")]
public record GcloudWorkbenchInstancesSetIamPolicyOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;