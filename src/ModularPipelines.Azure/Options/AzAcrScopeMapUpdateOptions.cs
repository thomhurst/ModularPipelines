using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "scope-map", "update")]
public record AzAcrScopeMapUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--add-gateway")]
    public string? AddGateway { get; set; }

    [CliOption("--add-repository")]
    public string? AddRepository { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--remove-gateway")]
    public string? RemoveGateway { get; set; }

    [CliOption("--remove-repository")]
    public string? RemoveRepository { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}