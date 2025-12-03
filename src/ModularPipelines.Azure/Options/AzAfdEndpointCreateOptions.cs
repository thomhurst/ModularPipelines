using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("afd", "endpoint", "create")]
public record AzAfdEndpointCreateOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--enabled-state")]
    public bool? EnabledState { get; set; }

    [CliOption("--name-reuse-scope")]
    public string? NameReuseScope { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}