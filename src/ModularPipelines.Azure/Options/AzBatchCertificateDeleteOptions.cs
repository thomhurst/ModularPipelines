using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "certificate", "delete")]
public record AzBatchCertificateDeleteOptions(
[property: CommandSwitch("--thumbprint")] string Thumbprint
) : AzOptions
{
    [BooleanCommandSwitch("--abort")]
    public bool? Abort { get; set; }

    [CommandSwitch("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}