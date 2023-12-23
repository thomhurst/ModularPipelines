using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "modify-account")]
public record AwsWorkspacesModifyAccountOptions : AwsOptions
{
    [CommandSwitch("--dedicated-tenancy-support")]
    public string? DedicatedTenancySupport { get; set; }

    [CommandSwitch("--dedicated-tenancy-management-cidr-range")]
    public string? DedicatedTenancyManagementCidrRange { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}