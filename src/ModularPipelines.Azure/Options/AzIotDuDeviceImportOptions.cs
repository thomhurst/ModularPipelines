using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "du", "device", "import")]
public record AzIotDuDeviceImportOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--instance")] string Instance
) : AzOptions
{
    [CliOption("--import-type")]
    public string? ImportType { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}