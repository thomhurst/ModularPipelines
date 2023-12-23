using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "start-test-execution")]
public record AwsLexv2ModelsStartTestExecutionOptions(
[property: CommandSwitch("--test-set-id")] string TestSetId,
[property: CommandSwitch("--target")] string Target,
[property: CommandSwitch("--api-mode")] string ApiMode
) : AwsOptions
{
    [CommandSwitch("--test-execution-modality")]
    public string? TestExecutionModality { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}