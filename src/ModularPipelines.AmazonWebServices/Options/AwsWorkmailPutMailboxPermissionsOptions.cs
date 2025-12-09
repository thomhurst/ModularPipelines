using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "put-mailbox-permissions")]
public record AwsWorkmailPutMailboxPermissionsOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--entity-id")] string EntityId,
[property: CliOption("--grantee-id")] string GranteeId,
[property: CliOption("--permission-values")] string[] PermissionValues
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}