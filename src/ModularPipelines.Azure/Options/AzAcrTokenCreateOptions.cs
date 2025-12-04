using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "token", "create")]
public record AzAcrTokenCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--expiration")]
    public string? Expiration { get; set; }

    [CliOption("--expiration-in-days")]
    public string? ExpirationInDays { get; set; }

    [CliOption("--gateway")]
    public string? Gateway { get; set; }

    [CliFlag("--no-passwords")]
    public bool? NoPasswords { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scope-map")]
    public string? ScopeMap { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }
}