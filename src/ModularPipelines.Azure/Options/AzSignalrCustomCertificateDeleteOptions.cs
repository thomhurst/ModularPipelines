using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signalr", "custom-certificate", "delete")]
public record AzSignalrCustomCertificateDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--signalr-name")] string SignalrName
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--keyvault-base-uri")]
    public string? KeyvaultBaseUri { get; set; }

    [CommandSwitch("--keyvault-secret-name")]
    public string? KeyvaultSecretName { get; set; }

    [CommandSwitch("--keyvault-secret-version")]
    public string? KeyvaultSecretVersion { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}

