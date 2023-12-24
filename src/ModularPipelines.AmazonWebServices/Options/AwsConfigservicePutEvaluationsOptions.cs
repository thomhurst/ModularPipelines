using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "put-evaluations")]
public record AwsConfigservicePutEvaluationsOptions(
[property: CommandSwitch("--result-token")] string ResultToken
) : AwsOptions
{
    [CommandSwitch("--evaluations")]
    public string[]? Evaluations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}