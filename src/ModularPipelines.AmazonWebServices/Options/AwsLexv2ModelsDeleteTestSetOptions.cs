using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "delete-test-set")]
public record AwsLexv2ModelsDeleteTestSetOptions(
[property: CommandSwitch("--test-set-id")] string TestSetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}