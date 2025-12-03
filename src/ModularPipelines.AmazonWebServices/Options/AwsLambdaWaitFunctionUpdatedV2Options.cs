using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "wait", "function-updated-v2")]
public record AwsLambdaWaitFunctionUpdatedV2Options(
[property: CliOption("--function-name")] string FunctionName
) : AwsOptions
{
    [CliOption("--qualifier")]
    public string? Qualifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}