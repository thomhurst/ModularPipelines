using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workstations", "configs", "get-iam-policy")]
public record GcloudWorkstationsConfigsGetIamPolicyOptions(
[property: PositionalArgument] string Config,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region
) : GcloudOptions;