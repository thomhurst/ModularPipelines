using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "list-share-invitations")]
public record AwsWellarchitectedListShareInvitationsOptions : AwsOptions
{
    [CommandSwitch("--workload-name-prefix")]
    public string? WorkloadNamePrefix { get; set; }

    [CommandSwitch("--lens-name-prefix")]
    public string? LensNamePrefix { get; set; }

    [CommandSwitch("--share-resource-type")]
    public string? ShareResourceType { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--profile-name-prefix")]
    public string? ProfileNamePrefix { get; set; }

    [CommandSwitch("--template-name-prefix")]
    public string? TemplateNamePrefix { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}