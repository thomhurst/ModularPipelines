using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "secret", "add")]
public record AzVmSecretAddOptions(
[property: CommandSwitch("--certificate")] string Certificate,
[property: CommandSwitch("--keyvault")] string Keyvault
) : AzOptions
{
    [CommandSwitch("--certificate-store")]
    public string? CertificateStore { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}