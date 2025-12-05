using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "job", "show")]
public record AzIotHubJobShowOptions(
[property: CliOption("--job-id")] string JobId
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}