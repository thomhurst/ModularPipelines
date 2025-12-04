using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "endpoint", "create", "eventhub")]
public record AzDtEndpointCreateEventhubOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--eh")] string Eh,
[property: CliOption("--ehn")] string Ehn,
[property: CliOption("--en")] string En
) : AzOptions
{
    [CliOption("--deadletter-sas-uri")]
    public string? DeadletterSasUri { get; set; }

    [CliOption("--deadletter-uri")]
    public string? DeadletterUri { get; set; }

    [CliOption("--ehg")]
    public string? Ehg { get; set; }

    [CliOption("--ehp")]
    public string? Ehp { get; set; }

    [CliOption("--ehs")]
    public string? Ehs { get; set; }

    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}