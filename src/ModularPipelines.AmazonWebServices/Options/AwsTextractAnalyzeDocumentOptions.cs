using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("textract", "analyze-document")]
public record AwsTextractAnalyzeDocumentOptions(
[property: CommandSwitch("--document")] string Document,
[property: CommandSwitch("--feature-types")] string[] FeatureTypes
) : AwsOptions
{
    [CommandSwitch("--human-loop-config")]
    public string? HumanLoopConfig { get; set; }

    [CommandSwitch("--queries-config")]
    public string? QueriesConfig { get; set; }

    [CommandSwitch("--adapters-config")]
    public string? AdaptersConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}