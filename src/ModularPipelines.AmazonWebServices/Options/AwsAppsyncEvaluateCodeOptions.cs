using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "evaluate-code")]
public record AwsAppsyncEvaluateCodeOptions(
[property: CliOption("--runtime")] string Runtime,
[property: CliOption("--code")] string Code,
[property: CliOption("--context")] string Context
) : AwsOptions
{
    [CliOption("--function")]
    public string? Function { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}