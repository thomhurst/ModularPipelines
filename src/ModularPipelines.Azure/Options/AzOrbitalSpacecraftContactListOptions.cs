using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("orbital", "spacecraft", "contact", "list")]
public record AzOrbitalSpacecraftContactListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--spacecraft-name")] string SpacecraftName
) : AzOptions
{
    [CliOption("--skiptoken")]
    public string? Skiptoken { get; set; }
}