using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "modify-db-parameter-group")]
public record AwsRdsModifyDbParameterGroupOptions(
[property: CliOption("--db-parameter-group-name")] string DbParameterGroupName,
[property: CliOption("--parameters")] string[] Parameters
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}