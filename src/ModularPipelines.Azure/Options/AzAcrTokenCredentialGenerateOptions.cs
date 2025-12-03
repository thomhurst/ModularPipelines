using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "token", "credential", "generate")]
public record AzAcrTokenCredentialGenerateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--expiration")]
    public string? Expiration { get; set; }

    [CliOption("--expiration-in-days")]
    public string? ExpirationInDays { get; set; }

    [CliFlag("--password1")]
    public bool? Password1 { get; set; }

    [CliFlag("--password2")]
    public bool? Password2 { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}