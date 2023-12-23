using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "create-cluster-parameter-group")]
public record AwsRedshiftCreateClusterParameterGroupOptions(
[property: CommandSwitch("--parameter-group-name")] string ParameterGroupName,
[property: CommandSwitch("--parameter-group-family")] string ParameterGroupFamily,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}