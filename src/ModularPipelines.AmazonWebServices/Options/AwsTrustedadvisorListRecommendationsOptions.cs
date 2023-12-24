using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("trustedadvisor", "list-recommendations")]
public record AwsTrustedadvisorListRecommendationsOptions : AwsOptions
{
    [CommandSwitch("--after-last-updated-at")]
    public long? AfterLastUpdatedAt { get; set; }

    [CommandSwitch("--aws-service")]
    public string? AwsService { get; set; }

    [CommandSwitch("--before-last-updated-at")]
    public long? BeforeLastUpdatedAt { get; set; }

    [CommandSwitch("--check-identifier")]
    public string? CheckIdentifier { get; set; }

    [CommandSwitch("--pillar")]
    public string? Pillar { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}