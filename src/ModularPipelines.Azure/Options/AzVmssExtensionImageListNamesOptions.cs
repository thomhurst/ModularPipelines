using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmss", "extension", "image", "list-names")]
public record AzVmssExtensionImageListNamesOptions : AzOptions
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