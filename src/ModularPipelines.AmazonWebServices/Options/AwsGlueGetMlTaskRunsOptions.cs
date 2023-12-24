using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-ml-task-runs")]
public record AwsGlueGetMlTaskRunsOptions(
[property: CommandSwitch("--transform-id")] string TransformId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--sort")]
    public string? Sort { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}