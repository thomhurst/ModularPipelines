using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "get-data-access")]
public record AwsS3controlGetDataAccessOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--target")] string Target,
[property: CliOption("--permission")] string Permission
) : AwsOptions
{
    [CliOption("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CliOption("--privilege")]
    public string? Privilege { get; set; }

    [CliOption("--target-type")]
    public string? TargetType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}