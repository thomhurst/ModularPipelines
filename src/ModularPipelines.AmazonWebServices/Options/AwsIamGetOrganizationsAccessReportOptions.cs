using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "get-organizations-access-report")]
public record AwsIamGetOrganizationsAccessReportOptions(
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--sort-key")]
    public string? SortKey { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}