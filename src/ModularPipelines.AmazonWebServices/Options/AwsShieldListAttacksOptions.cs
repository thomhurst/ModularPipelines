using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("shield", "list-attacks")]
public record AwsShieldListAttacksOptions : AwsOptions
{
    [CliOption("--resource-arns")]
    public string[]? ResourceArns { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}