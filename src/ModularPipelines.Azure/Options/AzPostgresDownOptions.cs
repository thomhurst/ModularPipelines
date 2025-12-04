using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("postgres", "down")]
public record AzPostgresDownOptions : AzOptions
{
    [CliOption("--delete-group")]
    public string? DeleteGroup { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server-name")]
    public string? ServerName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}