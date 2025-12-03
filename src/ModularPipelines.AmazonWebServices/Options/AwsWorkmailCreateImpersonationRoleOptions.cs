using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "create-impersonation-role")]
public record AwsWorkmailCreateImpersonationRoleOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type,
[property: CliOption("--rules")] string[] Rules
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}