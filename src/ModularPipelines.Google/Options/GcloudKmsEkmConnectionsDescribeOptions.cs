using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "ekm-connections", "describe")]
public record GcloudKmsEkmConnectionsDescribeOptions(
[property: PositionalArgument] string EkmConnection,
[property: PositionalArgument] string Location
) : GcloudOptions;