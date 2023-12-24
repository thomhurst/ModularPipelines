using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "modify-cluster-parameter-group")]
public record AwsRedshiftModifyClusterParameterGroupOptions(
[property: CommandSwitch("--parameter-group-name")] string ParameterGroupName,
[property: CommandSwitch("--parameters")] string[] Parameters
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}