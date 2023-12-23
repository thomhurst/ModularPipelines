using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "create-location-nfs")]
public record AwsDatasyncCreateLocationNfsOptions(
[property: CommandSwitch("--subdirectory")] string Subdirectory,
[property: CommandSwitch("--server-hostname")] string ServerHostname,
[property: CommandSwitch("--on-prem-config")] string OnPremConfig
) : AwsOptions
{
    [CommandSwitch("--mount-options")]
    public string? MountOptions { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}