using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "ekm-connections", "get-iam-policy")]
public record GcloudKmsEkmConnectionsGetIamPolicyOptions(
[property: CliArgument] string EkmConnection,
[property: CliArgument] string Location
) : GcloudOptions;