using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-managed-instance-role")]
public record AwsSsmUpdateManagedInstanceRoleOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--iam-role")] string IamRole
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}