using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediaconnect", "remove-flow-source")]
public record AwsMediaconnectRemoveFlowSourceOptions(
[property: CommandSwitch("--flow-arn")] string FlowArn,
[property: CommandSwitch("--source-arn")] string SourceArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}