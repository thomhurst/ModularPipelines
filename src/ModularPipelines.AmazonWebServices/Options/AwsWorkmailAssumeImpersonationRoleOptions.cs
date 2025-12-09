using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "assume-impersonation-role")]
public record AwsWorkmailAssumeImpersonationRoleOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--impersonation-role-id")] string ImpersonationRoleId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}