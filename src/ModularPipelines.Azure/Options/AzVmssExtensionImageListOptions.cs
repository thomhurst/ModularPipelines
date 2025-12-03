using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmss", "extension", "image", "list")]
public record AzVmssExtensionImageListOptions : AzOptions
{
    [CliFlag("--latest")]
    public bool? Latest { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--publisher")]
    public string? Publisher { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}