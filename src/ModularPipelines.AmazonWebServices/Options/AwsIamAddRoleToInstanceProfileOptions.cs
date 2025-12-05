using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "add-role-to-instance-profile")]
public record AwsIamAddRoleToInstanceProfileOptions(
[property: CliOption("--instance-profile-name")] string InstanceProfileName,
[property: CliOption("--role-name")] string RoleName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}