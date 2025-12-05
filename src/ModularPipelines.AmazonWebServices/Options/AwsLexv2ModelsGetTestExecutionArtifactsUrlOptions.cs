using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "get-test-execution-artifacts-url")]
public record AwsLexv2ModelsGetTestExecutionArtifactsUrlOptions(
[property: CliOption("--test-execution-id")] string TestExecutionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}