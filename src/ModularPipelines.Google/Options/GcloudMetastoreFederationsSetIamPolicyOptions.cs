using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "federations", "set-iam-policy")]
public record GcloudMetastoreFederationsSetIamPolicyOptions(
[property: PositionalArgument] string Federation,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;