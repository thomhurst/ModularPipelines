using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "certificate", "download")]
public record AzKeyvaultCertificateDownloadOptions(
[property: CommandSwitch("--file")] string File
) : AzOptions
{
    [CommandSwitch("--encoding")]
    public string? Encoding { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}