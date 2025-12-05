using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "delete-db-parameter-group")]
public record AwsRdsDeleteDbParameterGroupOptions(
[property: CliOption("--db-parameter-group-name")] string DbParameterGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}