using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "ca-certificate", "download-chain")]
public record AzSphereCaCertificateDownloadChainOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--output-file")] string OutputFile,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--serial-number")]
    public string? SerialNumber { get; set; }
}