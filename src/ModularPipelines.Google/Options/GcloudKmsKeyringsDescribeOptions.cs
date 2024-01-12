using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keyrings", "describe")]
public record GcloudKmsKeyringsDescribeOptions(
[property: PositionalArgument] string Keyring,
[property: PositionalArgument] string Location
) : GcloudOptions;