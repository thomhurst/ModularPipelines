using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "add-flow-sources")]
public record AwsMediaconnectAddFlowSourcesOptions(
[property: CommandSwitch("--flow-arn")] string FlowArn,
[property: CommandSwitch("--sources")] string[] Sources
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}