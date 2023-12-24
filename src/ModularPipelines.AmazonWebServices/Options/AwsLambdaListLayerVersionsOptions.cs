using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "list-layer-versions")]
public record AwsLambdaListLayerVersionsOptions(
[property: CommandSwitch("--layer-name")] string LayerName
) : AwsOptions
{
    [CommandSwitch("--compatible-runtime")]
    public string? CompatibleRuntime { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--compatible-architecture")]
    public string? CompatibleArchitecture { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}