using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "ekm-connections", "describe")]
public record GcloudKmsEkmConnectionsDescribeOptions(
[property: CliArgument] string EkmConnection,
[property: CliArgument] string Location
) : GcloudOptions;