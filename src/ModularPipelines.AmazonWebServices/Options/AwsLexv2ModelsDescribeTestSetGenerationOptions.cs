using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "describe-test-set-generation")]
public record AwsLexv2ModelsDescribeTestSetGenerationOptions(
[property: CliOption("--test-set-generation-id")] string TestSetGenerationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}