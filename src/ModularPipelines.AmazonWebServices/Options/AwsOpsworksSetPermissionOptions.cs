using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "set-permission")]
public record AwsOpsworksSetPermissionOptions(
[property: CommandSwitch("--stack-id")] string StackId,
[property: CommandSwitch("--iam-user-arn")] string IamUserArn
) : AwsOptions
{
    [CommandSwitch("--level")]
    public string? Level { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}