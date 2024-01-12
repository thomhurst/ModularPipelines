using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "ekm-connections", "get-iam-policy")]
public record GcloudKmsEkmConnectionsGetIamPolicyOptions(
[property: PositionalArgument] string EkmConnection,
[property: PositionalArgument] string Location
) : GcloudOptions;