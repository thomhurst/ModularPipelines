using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "update-input")]
public record AwsMedialiveUpdateInputOptions(
[property: CommandSwitch("--input-id")] string InputId
) : AwsOptions
{
    [CommandSwitch("--destinations")]
    public string[]? Destinations { get; set; }

    [CommandSwitch("--input-devices")]
    public string[]? InputDevices { get; set; }

    [CommandSwitch("--input-security-groups")]
    public string[]? InputSecurityGroups { get; set; }

    [CommandSwitch("--media-connect-flows")]
    public string[]? MediaConnectFlows { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--sources")]
    public string[]? Sources { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}