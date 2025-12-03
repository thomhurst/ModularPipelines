using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("staticwebapp", "identity", "remove")]
public record AzStaticwebappIdentityRemoveOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--identities")]
    public string? Identities { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}