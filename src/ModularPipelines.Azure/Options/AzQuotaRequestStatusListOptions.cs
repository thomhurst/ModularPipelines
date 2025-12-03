using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quota", "request", "status", "list")]
public record AzQuotaRequestStatusListOptions(
[property: CliOption("--scope")] string Scope
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}