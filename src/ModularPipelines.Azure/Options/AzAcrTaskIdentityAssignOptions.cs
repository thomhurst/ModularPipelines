using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "task", "identity", "assign")]
public record AzAcrTaskIdentityAssignOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--identities")]
    public string? Identities { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}