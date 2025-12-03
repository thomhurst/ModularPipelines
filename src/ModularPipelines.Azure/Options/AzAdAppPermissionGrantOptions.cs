using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "app", "permission", "grant")]
public record AzAdAppPermissionGrantOptions(
[property: CliOption("--api")] string Api,
[property: CliOption("--id")] string Id,
[property: CliOption("--scope")] string Scope
) : AzOptions
{
    [CliOption("--consent-type")]
    public string? ConsentType { get; set; }

    [CliOption("--principal-id")]
    public string? PrincipalId { get; set; }
}