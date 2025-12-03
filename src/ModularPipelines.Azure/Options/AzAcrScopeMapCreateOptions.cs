using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "scope-map", "create")]
public record AzAcrScopeMapCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--gateway")]
    public string? Gateway { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}