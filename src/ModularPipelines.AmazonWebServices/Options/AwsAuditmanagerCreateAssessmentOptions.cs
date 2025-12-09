using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auditmanager", "create-assessment")]
public record AwsAuditmanagerCreateAssessmentOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--assessment-reports-destination")] string AssessmentReportsDestination,
[property: CliOption("--scope")] string Scope,
[property: CliOption("--roles")] string[] Roles,
[property: CliOption("--framework-id")] string FrameworkId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}