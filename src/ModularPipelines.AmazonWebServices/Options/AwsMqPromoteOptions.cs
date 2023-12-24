using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mq", "promote")]
public record AwsMqPromoteOptions(
[property: CommandSwitch("--broker-id")] string BrokerId,
[property: CommandSwitch("--mode")] string Mode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}