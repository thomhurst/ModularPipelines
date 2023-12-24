using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "update-role-description")]
public record AwsIamUpdateRoleDescriptionOptions(
[property: CommandSwitch("--role-name")] string RoleName,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}