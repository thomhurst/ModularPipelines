using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "federations", "get-iam-policy")]
public record GcloudMetastoreFederationsGetIamPolicyOptions(
[property: PositionalArgument] string Federation,
[property: PositionalArgument] string Location
) : GcloudOptions;