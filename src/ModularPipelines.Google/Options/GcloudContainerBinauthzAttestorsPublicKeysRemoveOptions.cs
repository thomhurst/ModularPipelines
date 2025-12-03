using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "attestors", "public-keys", "remove")]
public record GcloudContainerBinauthzAttestorsPublicKeysRemoveOptions(
[property: CliArgument] string PublicKeyId,
[property: CliOption("--attestor")] string Attestor
) : GcloudOptions;