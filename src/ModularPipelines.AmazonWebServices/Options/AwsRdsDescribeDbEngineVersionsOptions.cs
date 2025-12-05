using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "describe-db-engine-versions")]
public record AwsRdsDescribeDbEngineVersionsOptions : AwsOptions
{
    [CliOption("--engine")]
    public string? Engine { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--db-parameter-group-family")]
    public string? DbParameterGroupFamily { get; set; }

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