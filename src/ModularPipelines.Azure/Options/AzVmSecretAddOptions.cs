using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

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
    public new string? Subscription { get; set; }
}