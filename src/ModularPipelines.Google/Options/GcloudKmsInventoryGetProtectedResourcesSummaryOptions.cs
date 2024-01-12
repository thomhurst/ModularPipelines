using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "inventory", "get-protected-resources-summary")]
public record GcloudKmsInventoryGetProtectedResourcesSummaryOptions(
[property: CommandSwitch("--keyname")] string Keyname,
[property: CommandSwitch("--keyring")] string Keyring,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;