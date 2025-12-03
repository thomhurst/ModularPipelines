using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "endpoint", "create", "servicebus")]
public record AzDtEndpointCreateServicebusOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--en")] string En,
[property: CliOption("--sbn")] string Sbn,
[property: CliOption("--sbt")] string Sbt
) : AzOptions
{
    [CliOption("--deadletter-sas-uri")]
    public string? DeadletterSasUri { get; set; }

    [CliOption("--deadletter-uri")]
    public string? DeadletterUri { get; set; }

    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sbg")]
    public string? Sbg { get; set; }

    [CliOption("--sbp")]
    public string? Sbp { get; set; }

    [CliOption("--sbs")]
    public string? Sbs { get; set; }
}