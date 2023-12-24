using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-incidents", "start-incident")]
public record AwsSsmIncidentsStartIncidentOptions(
[property: CommandSwitch("--response-plan-arn")] string ResponsePlanArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--impact")]
    public int? Impact { get; set; }

    [CommandSwitch("--related-items")]
    public string[]? RelatedItems { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }

    [CommandSwitch("--trigger-details")]
    public string? TriggerDetails { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}