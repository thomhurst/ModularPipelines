using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-db-parameter-group")]
public record AwsRdsCreateDbParameterGroupOptions(
[property: CliOption("--db-parameter-group-name")] string DbParameterGroupName,
[property: CliOption("--db-parameter-group-family")] string DbParameterGroupFamily,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}