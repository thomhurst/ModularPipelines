using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "delete-custom-db-engine-version")]
public record AwsRdsDeleteCustomDbEngineVersionOptions(
[property: CliOption("--engine")] string Engine,
[property: CliOption("--engine-version")] string EngineVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}