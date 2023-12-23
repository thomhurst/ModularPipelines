using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "create-image-builder")]
public record AwsAppstreamCreateImageBuilderOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--instance-type")] string InstanceType
) : AwsOptions
{
    [CommandSwitch("--image-name")]
    public string? ImageName { get; set; }

    [CommandSwitch("--image-arn")]
    public string? ImageArn { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--iam-role-arn")]
    public string? IamRoleArn { get; set; }

    [CommandSwitch("--domain-join-info")]
    public string? DomainJoinInfo { get; set; }

    [CommandSwitch("--appstream-agent-version")]
    public string? AppstreamAgentVersion { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--access-endpoints")]
    public string[]? AccessEndpoints { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}