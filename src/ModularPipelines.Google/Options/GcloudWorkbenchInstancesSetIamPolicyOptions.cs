using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workbench", "instances", "set-iam-policy")]
public record GcloudWorkbenchInstancesSetIamPolicyOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;