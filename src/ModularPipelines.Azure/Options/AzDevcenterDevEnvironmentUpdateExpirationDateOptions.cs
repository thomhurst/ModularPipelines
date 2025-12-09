using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "dev", "environment", "update-expiration-date")]
public record AzDevcenterDevEnvironmentUpdateExpirationDateOptions(
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--project")] string Project
) : AzOptions
{
    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--expiration")]
    public string? Expiration { get; set; }

    [CliOption("--user-id")]
    public string? UserId { get; set; }
}