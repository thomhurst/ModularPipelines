using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keyrings", "describe")]
public record GcloudKmsKeyringsDescribeOptions(
[property: CliArgument] string Keyring,
[property: CliArgument] string Location
) : GcloudOptions;