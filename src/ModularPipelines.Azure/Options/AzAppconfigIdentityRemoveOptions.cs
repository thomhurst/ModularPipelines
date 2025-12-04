using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "identity", "remove")]
public record AzAppconfigIdentityRemoveOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--identities")]
    public string? Identities { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}