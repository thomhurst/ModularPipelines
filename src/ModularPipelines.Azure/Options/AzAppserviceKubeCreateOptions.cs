using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appservice", "kube", "create")]
public record AzAppserviceKubeCreateOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--static-ip")]
    public string? StaticIp { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}