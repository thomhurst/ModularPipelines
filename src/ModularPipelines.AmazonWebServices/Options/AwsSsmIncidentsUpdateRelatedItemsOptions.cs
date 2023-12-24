using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-incidents", "update-related-items")]
public record AwsSsmIncidentsUpdateRelatedItemsOptions(
[property: CommandSwitch("--incident-record-arn")] string IncidentRecordArn,
[property: CommandSwitch("--related-items-update")] string RelatedItemsUpdate
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}