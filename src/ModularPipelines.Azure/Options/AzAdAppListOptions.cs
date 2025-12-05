using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "app", "list")]
public record AzAdAppListOptions : AzOptions
{
    [CliOption("--all")]
    public string? All { get; set; }

    [CliOption("--app-id")]
    public string? AppId { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--identifier-uri")]
    public string? IdentifierUri { get; set; }

    [CliOption("--show-mine")]
    public string? ShowMine { get; set; }
}