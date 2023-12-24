using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "update-app-block-builder")]
public record AwsAppstreamUpdateAppBlockBuilderOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--iam-role-arn")]
    public string? IamRoleArn { get; set; }

    [CommandSwitch("--access-endpoints")]
    public string[]? AccessEndpoints { get; set; }

    [CommandSwitch("--attributes-to-delete")]
    public string[]? AttributesToDelete { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}