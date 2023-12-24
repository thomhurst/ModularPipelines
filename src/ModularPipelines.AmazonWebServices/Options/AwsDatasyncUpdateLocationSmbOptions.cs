using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "update-location-smb")]
public record AwsDatasyncUpdateLocationSmbOptions(
[property: CommandSwitch("--location-arn")] string LocationArn
) : AwsOptions
{
    [CommandSwitch("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--agent-arns")]
    public string[]? AgentArns { get; set; }

    [CommandSwitch("--mount-options")]
    public string? MountOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}