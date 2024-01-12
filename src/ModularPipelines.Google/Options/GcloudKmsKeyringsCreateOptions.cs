using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keyrings", "create")]
public record GcloudKmsKeyringsCreateOptions(
[property: PositionalArgument] string Keyring,
[property: PositionalArgument] string Location
) : GcloudOptions;