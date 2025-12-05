using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("databoxedge", "device", "create")]
public record AzDataboxedgeDeviceCreateOptions(
[property: CliOption("--device-name")] string DeviceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--model-description")]
    public string? ModelDescription { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}