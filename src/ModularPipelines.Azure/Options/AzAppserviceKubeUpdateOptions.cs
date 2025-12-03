using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appservice", "kube", "update")]
public record AzAppserviceKubeUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--custom-location")]
    public string? CustomLocation { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--static-ip")]
    public string? StaticIp { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}