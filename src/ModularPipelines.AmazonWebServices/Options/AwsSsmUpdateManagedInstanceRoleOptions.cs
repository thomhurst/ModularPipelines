using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "update-managed-instance-role")]
public record AwsSsmUpdateManagedInstanceRoleOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--iam-role")] string IamRole
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}