using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("internet-analyzer", "test", "update")]
public record AzInternetAnalyzerTestUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enabled-state")]
    public bool? EnabledState { get; set; }

    [CliOption("--endpoint-a-endpoint")]
    public string? EndpointAEndpoint { get; set; }

    [CliOption("--endpoint-a-name")]
    public string? EndpointAName { get; set; }

    [CliOption("--endpoint-b-endpoint")]
    public string? EndpointBEndpoint { get; set; }

    [CliOption("--endpoint-b-name")]
    public string? EndpointBName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}