using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "inventory", "search-protected-resources")]
public record GcloudKmsInventorySearchProtectedResourcesOptions(
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--keyname")] string Keyname,
[property: CommandSwitch("--keyring")] string Keyring,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions
{
    [CommandSwitch("--resource-types")]
    public string[]? ResourceTypes { get; set; }
}