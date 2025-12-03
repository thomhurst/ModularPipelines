using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "inventory", "get-protected-resources-summary")]
public record GcloudKmsInventoryGetProtectedResourcesSummaryOptions(
[property: CliOption("--keyname")] string Keyname,
[property: CliOption("--keyring")] string Keyring,
[property: CliOption("--location")] string Location
) : GcloudOptions;