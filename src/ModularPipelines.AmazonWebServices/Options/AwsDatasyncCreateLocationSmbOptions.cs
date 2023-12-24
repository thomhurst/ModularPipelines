using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "create-location-smb")]
public record AwsDatasyncCreateLocationSmbOptions(
[property: CommandSwitch("--subdirectory")] string Subdirectory,
[property: CommandSwitch("--server-hostname")] string ServerHostname,
[property: CommandSwitch("--user")] string User,
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--agent-arns")] string[] AgentArns
) : AwsOptions
{
    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--mount-options")]
    public string? MountOptions { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}