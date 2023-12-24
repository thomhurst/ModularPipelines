using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "update-test-set")]
public record AwsLexv2ModelsUpdateTestSetOptions(
[property: CommandSwitch("--test-set-id")] string TestSetId,
[property: CommandSwitch("--test-set-name")] string TestSetName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}