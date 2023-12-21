using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connected-env", "certificate", "delete")]
public record AzContainerappConnectedEnvCertificateDeleteOptions : AzOptions
{
    [CommandSwitch("--certificate")]
    public string? Certificate { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--thumbprint")]
    public string? Thumbprint { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}