using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "create-software-update-job")]
public record AwsGreengrassCreateSoftwareUpdateJobOptions(
[property: CommandSwitch("--s3-url-signer-role")] string S3UrlSignerRole,
[property: CommandSwitch("--software-to-update")] string SoftwareToUpdate,
[property: CommandSwitch("--update-targets")] string[] UpdateTargets,
[property: CommandSwitch("--update-targets-architecture")] string UpdateTargetsArchitecture,
[property: CommandSwitch("--update-targets-operating-system")] string UpdateTargetsOperatingSystem
) : AwsOptions
{
    [CommandSwitch("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CommandSwitch("--update-agent-log-level")]
    public string? UpdateAgentLogLevel { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}