using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "dev", "environment", "delay-action")]
public record AzDevcenterDevEnvironmentDelayActionOptions(
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--delay-time")] string DelayTime,
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--project")] string Project
) : AzOptions
{
    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--user-id")]
    public string? UserId { get; set; }
}