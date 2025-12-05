using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune", "describe-orderable-db-instance-options")]
public record AwsNeptuneDescribeOrderableDbInstanceOptionsOptions(
[property: CliOption("--engine")] string Engine
) : AwsOptions
{
    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--db-instance-class")]
    public string? DbInstanceClass { get; set; }

    [CliOption("--license-model")]
    public string? LicenseModel { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}