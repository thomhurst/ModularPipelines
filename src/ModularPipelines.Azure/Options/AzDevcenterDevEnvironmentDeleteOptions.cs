using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "dev", "environment", "delete")]
public record AzDevcenterDevEnvironmentDeleteOptions(
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--project")] string Project
) : AzOptions
{
    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--user-id")]
    public string? UserId { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}