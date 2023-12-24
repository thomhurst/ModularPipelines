using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "put-role-policy")]
public record AwsIamPutRolePolicyOptions(
[property: CommandSwitch("--role-name")] string RoleName,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--policy-document")] string PolicyDocument
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}