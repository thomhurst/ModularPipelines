using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "delete-compute-environment")]
public record AwsBatchDeleteComputeEnvironmentOptions(
[property: CommandSwitch("--compute-environment")] string ComputeEnvironment
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}