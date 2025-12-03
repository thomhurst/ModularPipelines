using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "put-compliance-items")]
public record AwsSsmPutComplianceItemsOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--compliance-type")] string ComplianceType,
[property: CliOption("--execution-summary")] string ExecutionSummary,
[property: CliOption("--items")] string[] Items
) : AwsOptions
{
    [CliOption("--item-content-hash")]
    public string? ItemContentHash { get; set; }

    [CliOption("--upload-type")]
    public string? UploadType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}