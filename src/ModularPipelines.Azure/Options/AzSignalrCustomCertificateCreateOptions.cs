using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signalr", "custom-certificate", "create")]
public record AzSignalrCustomCertificateCreateOptions(
[property: CommandSwitch("--keyvault-base-uri")] string KeyvaultBaseUri,
[property: CommandSwitch("--keyvault-secret-name")] string KeyvaultSecretName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--signalr-name")] string SignalrName
) : AzOptions
{
    [CommandSwitch("--keyvault-secret-version")]
    public string? KeyvaultSecretVersion { get; set; }
}