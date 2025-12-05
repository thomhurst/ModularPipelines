using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "inventory", "search-protected-resources")]
public record GcloudKmsInventorySearchProtectedResourcesOptions(
[property: CliOption("--scope")] string Scope,
[property: CliOption("--keyname")] string Keyname,
[property: CliOption("--keyring")] string Keyring,
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliOption("--resource-types")]
    public string[]? ResourceTypes { get; set; }
}