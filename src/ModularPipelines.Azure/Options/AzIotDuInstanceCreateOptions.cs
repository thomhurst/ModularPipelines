using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "du", "instance", "create")]
public record AzIotDuInstanceCreateOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--instance")] string Instance,
[property: CliOption("--iothub-ids")] string IothubIds
) : AzOptions
{
    [CliOption("--diagnostics-storage-id")]
    public string? DiagnosticsStorageId { get; set; }

    [CliFlag("--enable-diagnostics")]
    public bool? EnableDiagnostics { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}