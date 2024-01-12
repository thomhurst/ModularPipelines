using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workstations", "configs", "set-iam-policy")]
public record GcloudWorkstationsConfigsSetIamPolicyOptions(
[property: PositionalArgument] string Config,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;