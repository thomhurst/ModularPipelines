using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune", "create-db-parameter-group")]
public record AwsNeptuneCreateDbParameterGroupOptions(
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