using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("desktopvirtualization", "applicationgroup", "create")]
public record AzDesktopvirtualizationApplicationgroupCreateOptions(
[property: CliOption("--application-group-type")] string ApplicationGroupType,
[property: CliOption("--host-pool-arm-path")] string HostPoolArmPath,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}