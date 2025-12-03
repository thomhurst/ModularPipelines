using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "describe-option-groups")]
public record AwsRdsDescribeOptionGroupsOptions : AwsOptions
{
    [CliOption("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--engine-name")]
    public string? EngineName { get; set; }

    [CliOption("--major-engine-version")]
    public string? MajorEngineVersion { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}