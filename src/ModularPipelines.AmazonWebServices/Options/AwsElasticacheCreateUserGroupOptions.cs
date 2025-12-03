using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "create-user-group")]
public record AwsElasticacheCreateUserGroupOptions(
[property: CliOption("--user-group-id")] string UserGroupId,
[property: CliOption("--engine")] string Engine
) : AwsOptions
{
    [CliOption("--user-ids")]
    public string[]? UserIds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}