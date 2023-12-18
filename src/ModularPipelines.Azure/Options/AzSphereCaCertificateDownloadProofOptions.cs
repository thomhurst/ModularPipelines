using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "ca-certificate", "download-proof")]
public record AzSphereCaCertificateDownloadProofOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--output-file")] string OutputFile,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--verification-code")] string VerificationCode
) : AzOptions
{
    [CommandSwitch("--serial-number")]
    public string? SerialNumber { get; set; }
}

