using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-incidents", "start-incident")]
public record AwsSsmIncidentsStartIncidentOptions(
[property: CliOption("--response-plan-arn")] string ResponsePlanArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--impact")]
    public int? Impact { get; set; }

    [CliOption("--related-items")]
    public string[]? RelatedItems { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--trigger-details")]
    public string? TriggerDetails { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}