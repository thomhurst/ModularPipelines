using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-dataflow-graph")]
public record AwsGlueGetDataflowGraphOptions : AwsOptions
{
    [CommandSwitch("--python-script")]
    public string? PythonScript { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}