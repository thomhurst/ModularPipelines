using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "describe-test-set-generation")]
public record AwsLexv2ModelsDescribeTestSetGenerationOptions(
[property: CommandSwitch("--test-set-generation-id")] string TestSetGenerationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}