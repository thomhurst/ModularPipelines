using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "create-launch-profile")]
public record AwsNimbleCreateLaunchProfileOptions(
[property: CliOption("--ec2-subnet-ids")] string[] Ec2SubnetIds,
[property: CliOption("--launch-profile-protocol-versions")] string[] LaunchProfileProtocolVersions,
[property: CliOption("--name")] string Name,
[property: CliOption("--stream-configuration")] string StreamConfiguration,
[property: CliOption("--studio-component-ids")] string[] StudioComponentIds,
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}