using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "delete-custom-db-engine-version")]
public record AwsRdsDeleteCustomDbEngineVersionOptions(
[property: CommandSwitch("--engine")] string Engine,
[property: CommandSwitch("--engine-version")] string EngineVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}