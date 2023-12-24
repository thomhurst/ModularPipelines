using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhuborchestrator", "list-workflows")]
public record AwsMigrationhuborchestratorListWorkflowsOptions : AwsOptions
{
    [CommandSwitch("--template-id")]
    public string? TemplateId { get; set; }

    [CommandSwitch("--ads-application-configuration-name")]
    public string? AdsApplicationConfigurationName { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}