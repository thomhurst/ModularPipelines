using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "update-security-control")]
public record AwsSecurityhubUpdateSecurityControlOptions(
[property: CliOption("--security-control-id")] string SecurityControlId,
[property: CliOption("--parameters")] IEnumerable<KeyValue> Parameters
) : AwsOptions
{
    [CliOption("--last-update-reason")]
    public string? LastUpdateReason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}