using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "ekm-connections", "set-iam-policy")]
public record GcloudKmsEkmConnectionsSetIamPolicyOptions(
[property: CliArgument] string EkmConnection,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;