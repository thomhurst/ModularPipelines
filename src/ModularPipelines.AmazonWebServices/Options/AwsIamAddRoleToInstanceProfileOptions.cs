using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "add-role-to-instance-profile")]
public record AwsIamAddRoleToInstanceProfileOptions(
[property: CommandSwitch("--instance-profile-name")] string InstanceProfileName,
[property: CommandSwitch("--role-name")] string RoleName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}