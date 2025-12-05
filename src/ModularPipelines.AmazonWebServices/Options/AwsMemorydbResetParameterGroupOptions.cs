using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "reset-parameter-group")]
public record AwsMemorydbResetParameterGroupOptions(
[property: CliOption("--parameter-group-name")] string ParameterGroupName
) : AwsOptions
{
    [CliOption("--parameter-names")]
    public string[]? ParameterNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}