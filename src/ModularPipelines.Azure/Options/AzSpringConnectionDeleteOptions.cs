using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "connection", "delete")]
public record AzSpringConnectionDeleteOptions : AzOptions
{
    [CliOption("--app")]
    public string? App { get; set; }

    [CliOption("--connection")]
    public string? Connection { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}