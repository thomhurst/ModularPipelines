using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "modify-db-snapshot")]
public record AwsRdsModifyDbSnapshotOptions(
[property: CliOption("--db-snapshot-identifier")] string DbSnapshotIdentifier
) : AwsOptions
{
    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}