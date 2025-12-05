using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "remove-role-from-instance-profile")]
public record AwsIamRemoveRoleFromInstanceProfileOptions(
[property: CliOption("--instance-profile-name")] string InstanceProfileName,
[property: CliOption("--role-name")] string RoleName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}