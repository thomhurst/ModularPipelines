using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "reset-cluster-parameter-group")]
public record AwsRedshiftResetClusterParameterGroupOptions(
[property: CommandSwitch("--parameter-group-name")] string ParameterGroupName
) : AwsOptions
{
    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}