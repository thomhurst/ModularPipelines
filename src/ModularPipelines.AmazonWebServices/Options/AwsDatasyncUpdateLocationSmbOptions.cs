using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "update-location-smb")]
public record AwsDatasyncUpdateLocationSmbOptions(
[property: CliOption("--location-arn")] string LocationArn
) : AwsOptions
{
    [CliOption("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CliOption("--user")]
    public string? User { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--agent-arns")]
    public string[]? AgentArns { get; set; }

    [CliOption("--mount-options")]
    public string? MountOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}