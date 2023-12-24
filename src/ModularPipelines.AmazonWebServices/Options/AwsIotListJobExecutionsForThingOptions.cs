using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "list-job-executions-for-thing")]
public record AwsIotListJobExecutionsForThingOptions(
[property: CommandSwitch("--thing-name")] string ThingName
) : AwsOptions
{
    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--namespace-id")]
    public string? NamespaceId { get; set; }

    [CommandSwitch("--job-id")]
    public string? JobId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}