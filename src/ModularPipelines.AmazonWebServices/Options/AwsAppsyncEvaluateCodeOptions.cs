using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "evaluate-code")]
public record AwsAppsyncEvaluateCodeOptions(
[property: CommandSwitch("--runtime")] string Runtime,
[property: CommandSwitch("--code")] string Code,
[property: CommandSwitch("--context")] string Context
) : AwsOptions
{
    [CommandSwitch("--function")]
    public string? Function { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}