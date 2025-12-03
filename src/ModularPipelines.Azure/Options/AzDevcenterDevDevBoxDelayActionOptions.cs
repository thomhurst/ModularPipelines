using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "dev", "dev-box", "delay-action")]
public record AzDevcenterDevDevBoxDelayActionOptions(
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--delay-time")] string DelayTime,
[property: CliOption("--dev-box-name")] string DevBoxName,
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