using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("signalr", "custom-certificate", "update")]
public record AzSignalrCustomCertificateUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--signalr-name")] string SignalrName
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--keyvault-base-uri")]
    public string? KeyvaultBaseUri { get; set; }

    [CliOption("--keyvault-secret-name")]
    public string? KeyvaultSecretName { get; set; }

    [CliOption("--keyvault-secret-version")]
    public string? KeyvaultSecretVersion { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}