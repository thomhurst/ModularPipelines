using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "update-impersonation-role")]
public record AwsWorkmailUpdateImpersonationRoleOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--impersonation-role-id")] string ImpersonationRoleId,
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type,
[property: CliOption("--rules")] string[] Rules
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}