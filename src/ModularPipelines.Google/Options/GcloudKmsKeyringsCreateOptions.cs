using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keyrings", "create")]
public record GcloudKmsKeyringsCreateOptions(
[property: CliArgument] string Keyring,
[property: CliArgument] string Location
) : GcloudOptions;