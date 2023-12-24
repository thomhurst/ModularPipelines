using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "describe-audit-suppression")]
public record AwsIotDescribeAuditSuppressionOptions(
[property: CommandSwitch("--check-name")] string CheckName,
[property: CommandSwitch("--resource-identifier")] string ResourceIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}