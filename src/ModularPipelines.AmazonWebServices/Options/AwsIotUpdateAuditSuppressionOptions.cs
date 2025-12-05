using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-audit-suppression")]
public record AwsIotUpdateAuditSuppressionOptions(
[property: CliOption("--check-name")] string CheckName,
[property: CliOption("--resource-identifier")] string ResourceIdentifier
) : AwsOptions
{
    [CliOption("--expiration-date")]
    public long? ExpirationDate { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}