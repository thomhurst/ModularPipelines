using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signalr", "create")]
public record AzSignalrCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliFlag("--allowed-origins")]
    public bool? AllowedOrigins { get; set; }

    [CliOption("--default-action")]
    public string? DefaultAction { get; set; }

    [CliFlag("--enable-message-logs")]
    public bool? EnableMessageLogs { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--service-mode")]
    public string? ServiceMode { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--unit-count")]
    public int? UnitCount { get; set; }
}