using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "start-assessment-framework-share")]
public record AwsAuditmanagerStartAssessmentFrameworkShareOptions(
[property: CliOption("--framework-id")] string FrameworkId,
[property: CliOption("--destination-account")] string DestinationAccount,
[property: CliOption("--destination-region")] string DestinationRegion
) : AwsOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}