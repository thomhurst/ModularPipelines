using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "connection", "delete")]
public record AzContainerappConnectionDeleteOptions : AzOptions
{
    [CliOption("--connection")]
    public string? Connection { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}