using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "update-control")]
public record AwsAuditmanagerUpdateControlOptions(
[property: CliOption("--control-id")] string ControlId,
[property: CliOption("--name")] string Name,
[property: CliOption("--control-mapping-sources")] string[] ControlMappingSources
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--testing-information")]
    public string? TestingInformation { get; set; }

    [CliOption("--action-plan-title")]
    public string? ActionPlanTitle { get; set; }

    [CliOption("--action-plan-instructions")]
    public string? ActionPlanInstructions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}