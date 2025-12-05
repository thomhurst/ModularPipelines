using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "list")]
public record GcloudKmsKeysListOptions(
[property: CliOption("--keyring")] string Keyring,
[property: CliOption("--location")] string Location
) : GcloudOptions;