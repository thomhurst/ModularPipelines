using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "endpoint", "create", "eventgrid")]
public record AzDtEndpointCreateEventgridOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--egt")] string Egt,
[property: CliOption("--en")] string En
) : AzOptions
{
    [CliOption("--deadletter-sas-uri")]
    public string? DeadletterSasUri { get; set; }

    [CliOption("--deadletter-uri")]
    public string? DeadletterUri { get; set; }

    [CliOption("--egg")]
    public string? Egg { get; set; }

    [CliOption("--egs")]
    public string? Egs { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}