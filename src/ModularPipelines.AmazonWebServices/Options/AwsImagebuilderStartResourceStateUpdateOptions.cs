using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "start-resource-state-update")]
public record AwsImagebuilderStartResourceStateUpdateOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--state")] string State
) : AwsOptions
{
    [CliOption("--execution-role")]
    public string? ExecutionRole { get; set; }

    [CliOption("--include-resources")]
    public string? IncludeResources { get; set; }

    [CliOption("--exclusion-rules")]
    public string? ExclusionRules { get; set; }

    [CliOption("--update-at")]
    public long? UpdateAt { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}