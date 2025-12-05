using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "create-software-update-job")]
public record AwsGreengrassCreateSoftwareUpdateJobOptions(
[property: CliOption("--s3-url-signer-role")] string S3UrlSignerRole,
[property: CliOption("--software-to-update")] string SoftwareToUpdate,
[property: CliOption("--update-targets")] string[] UpdateTargets,
[property: CliOption("--update-targets-architecture")] string UpdateTargetsArchitecture,
[property: CliOption("--update-targets-operating-system")] string UpdateTargetsOperatingSystem
) : AwsOptions
{
    [CliOption("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CliOption("--update-agent-log-level")]
    public string? UpdateAgentLogLevel { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}