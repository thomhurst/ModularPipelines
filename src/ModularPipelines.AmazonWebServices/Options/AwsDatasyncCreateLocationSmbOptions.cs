using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "create-location-smb")]
public record AwsDatasyncCreateLocationSmbOptions(
[property: CliOption("--subdirectory")] string Subdirectory,
[property: CliOption("--server-hostname")] string ServerHostname,
[property: CliOption("--user")] string User,
[property: CliOption("--password")] string Password,
[property: CliOption("--agent-arns")] string[] AgentArns
) : AwsOptions
{
    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--mount-options")]
    public string? MountOptions { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}