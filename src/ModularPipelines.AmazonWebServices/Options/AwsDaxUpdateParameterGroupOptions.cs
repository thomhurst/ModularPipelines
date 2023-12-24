using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dax", "update-parameter-group")]
public record AwsDaxUpdateParameterGroupOptions(
[property: CommandSwitch("--parameter-group-name")] string ParameterGroupName,
[property: CommandSwitch("--parameter-name-values")] string[] ParameterNameValues
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}