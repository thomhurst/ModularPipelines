using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "modify-account")]
public record AwsWorkspacesModifyAccountOptions : AwsOptions
{
    [CliOption("--dedicated-tenancy-support")]
    public string? DedicatedTenancySupport { get; set; }

    [CliOption("--dedicated-tenancy-management-cidr-range")]
    public string? DedicatedTenancyManagementCidrRange { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}