using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("signalr", "custom-certificate", "create")]
public record AzSignalrCustomCertificateCreateOptions(
[property: CliOption("--keyvault-base-uri")] string KeyvaultBaseUri,
[property: CliOption("--keyvault-secret-name")] string KeyvaultSecretName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--signalr-name")] string SignalrName
) : AzOptions
{
    [CliOption("--keyvault-secret-version")]
    public string? KeyvaultSecretVersion { get; set; }
}