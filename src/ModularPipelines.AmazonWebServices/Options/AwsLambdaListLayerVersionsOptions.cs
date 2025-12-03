using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "list-layer-versions")]
public record AwsLambdaListLayerVersionsOptions(
[property: CliOption("--layer-name")] string LayerName
) : AwsOptions
{
    [CliOption("--compatible-runtime")]
    public string? CompatibleRuntime { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--compatible-architecture")]
    public string? CompatibleArchitecture { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}