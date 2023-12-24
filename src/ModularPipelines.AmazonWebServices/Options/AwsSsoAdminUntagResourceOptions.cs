using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "untag-resource")]
public record AwsSsoAdminUntagResourceOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--tag-keys")] string[] TagKeys
) : AwsOptions
{
    [CommandSwitch("--instance-arn")]
    public string? InstanceArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}