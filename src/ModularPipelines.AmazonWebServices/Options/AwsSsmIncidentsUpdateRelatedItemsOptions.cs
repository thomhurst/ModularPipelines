using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-incidents", "update-related-items")]
public record AwsSsmIncidentsUpdateRelatedItemsOptions(
[property: CliOption("--incident-record-arn")] string IncidentRecordArn,
[property: CliOption("--related-items-update")] string RelatedItemsUpdate
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}