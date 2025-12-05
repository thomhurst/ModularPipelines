using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "reset-db-parameter-group")]
public record AwsRdsResetDbParameterGroupOptions(
[property: CliOption("--db-parameter-group-name")] string DbParameterGroupName
) : AwsOptions
{
    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}