using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "list-share-invitations")]
public record AwsWellarchitectedListShareInvitationsOptions : AwsOptions
{
    [CliOption("--workload-name-prefix")]
    public string? WorkloadNamePrefix { get; set; }

    [CliOption("--lens-name-prefix")]
    public string? LensNamePrefix { get; set; }

    [CliOption("--share-resource-type")]
    public string? ShareResourceType { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--profile-name-prefix")]
    public string? ProfileNamePrefix { get; set; }

    [CliOption("--template-name-prefix")]
    public string? TemplateNamePrefix { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}