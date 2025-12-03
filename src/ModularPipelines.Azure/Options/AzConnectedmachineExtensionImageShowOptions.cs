using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectedmachine", "extension", "image", "show")]
public record AzConnectedmachineExtensionImageShowOptions : AzOptions
{
    [CliOption("--extension-type")]
    public string? ExtensionType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--publisher")]
    public string? Publisher { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}