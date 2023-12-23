using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafkaconnect", "delete-custom-plugin")]
public record AwsKafkaconnectDeleteCustomPluginOptions(
[property: CommandSwitch("--custom-plugin-arn")] string CustomPluginArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}