using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memorydb", "reset-parameter-group")]
public record AwsMemorydbResetParameterGroupOptions(
[property: CommandSwitch("--parameter-group-name")] string ParameterGroupName
) : AwsOptions
{
    [CommandSwitch("--parameter-names")]
    public string[]? ParameterNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}