using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "generate-organizations-access-report")]
public record AwsIamGenerateOrganizationsAccessReportOptions(
[property: CliOption("--entity-path")] string EntityPath
) : AwsOptions
{
    [CliOption("--organizations-policy-id")]
    public string? OrganizationsPolicyId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}