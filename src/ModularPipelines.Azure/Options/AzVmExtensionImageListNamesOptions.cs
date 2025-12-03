using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "extension", "image", "list-names")]
public record AzVmExtensionImageListNamesOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--publisher")]
    public string? Publisher { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}