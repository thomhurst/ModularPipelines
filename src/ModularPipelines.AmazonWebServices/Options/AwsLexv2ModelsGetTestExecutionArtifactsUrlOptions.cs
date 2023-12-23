using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "get-test-execution-artifacts-url")]
public record AwsLexv2ModelsGetTestExecutionArtifactsUrlOptions(
[property: CommandSwitch("--test-execution-id")] string TestExecutionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}