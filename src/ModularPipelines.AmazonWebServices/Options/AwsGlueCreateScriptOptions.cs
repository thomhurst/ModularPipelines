using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "create-script")]
public record AwsGlueCreateScriptOptions : AwsOptions
{
    [CommandSwitch("--dag-nodes")]
    public string[]? DagNodes { get; set; }

    [CommandSwitch("--dag-edges")]
    public string[]? DagEdges { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}