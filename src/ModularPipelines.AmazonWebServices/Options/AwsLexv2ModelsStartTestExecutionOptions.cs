using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "start-test-execution")]
public record AwsLexv2ModelsStartTestExecutionOptions(
[property: CliOption("--test-set-id")] string TestSetId,
[property: CliOption("--target")] string Target,
[property: CliOption("--api-mode")] string ApiMode
) : AwsOptions
{
    [CliOption("--test-execution-modality")]
    public string? TestExecutionModality { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}