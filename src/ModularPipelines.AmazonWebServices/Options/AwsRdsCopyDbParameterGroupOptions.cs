using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "copy-db-parameter-group")]
public record AwsRdsCopyDbParameterGroupOptions(
[property: CliOption("--source-db-parameter-group-identifier")] string SourceDbParameterGroupIdentifier,
[property: CliOption("--target-db-parameter-group-identifier")] string TargetDbParameterGroupIdentifier,
[property: CliOption("--target-db-parameter-group-description")] string TargetDbParameterGroupDescription
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}