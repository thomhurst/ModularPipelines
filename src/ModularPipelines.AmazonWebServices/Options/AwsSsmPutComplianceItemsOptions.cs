using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "put-compliance-items")]
public record AwsSsmPutComplianceItemsOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--compliance-type")] string ComplianceType,
[property: CommandSwitch("--execution-summary")] string ExecutionSummary,
[property: CommandSwitch("--items")] string[] Items
) : AwsOptions
{
    [CommandSwitch("--item-content-hash")]
    public string? ItemContentHash { get; set; }

    [CommandSwitch("--upload-type")]
    public string? UploadType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}