using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "batch-put-property-values")]
public record AwsIottwinmakerBatchPutPropertyValuesOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--entries")] string[] Entries
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}