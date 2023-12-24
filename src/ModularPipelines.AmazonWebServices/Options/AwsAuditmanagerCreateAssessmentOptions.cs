using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auditmanager", "create-assessment")]
public record AwsAuditmanagerCreateAssessmentOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--assessment-reports-destination")] string AssessmentReportsDestination,
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--roles")] string[] Roles,
[property: CommandSwitch("--framework-id")] string FrameworkId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}