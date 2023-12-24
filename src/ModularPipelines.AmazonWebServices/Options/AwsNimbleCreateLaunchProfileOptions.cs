using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nimble", "create-launch-profile")]
public record AwsNimbleCreateLaunchProfileOptions(
[property: CommandSwitch("--ec2-subnet-ids")] string[] Ec2SubnetIds,
[property: CommandSwitch("--launch-profile-protocol-versions")] string[] LaunchProfileProtocolVersions,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--stream-configuration")] string StreamConfiguration,
[property: CommandSwitch("--studio-component-ids")] string[] StudioComponentIds,
[property: CommandSwitch("--studio-id")] string StudioId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}