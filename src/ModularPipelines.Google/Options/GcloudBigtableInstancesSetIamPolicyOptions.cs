using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "set-iam-policy")]
public record GcloudBigtableInstancesSetIamPolicyOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;