using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "clusters", "get-credentials")]
public record GcloudContainerClustersGetCredentialsOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--internal-ip")]
    public bool? InternalIp { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}