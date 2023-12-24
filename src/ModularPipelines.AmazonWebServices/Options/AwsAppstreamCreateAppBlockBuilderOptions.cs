using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "create-app-block-builder")]
public record AwsAppstreamCreateAppBlockBuilderOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--platform")] string Platform,
[property: CommandSwitch("--instance-type")] string InstanceType,
[property: CommandSwitch("--vpc-config")] string VpcConfig
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--iam-role-arn")]
    public string? IamRoleArn { get; set; }

    [CommandSwitch("--access-endpoints")]
    public string[]? AccessEndpoints { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}