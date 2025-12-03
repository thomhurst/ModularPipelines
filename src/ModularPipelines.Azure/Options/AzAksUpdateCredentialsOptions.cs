using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "update-credentials")]
public record AzAksUpdateCredentialsOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--client-secret")]
    public string? ClientSecret { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--reset-service-principal")]
    public bool? ResetServicePrincipal { get; set; }

    [CliOption("--service-principal")]
    public string? ServicePrincipal { get; set; }
}