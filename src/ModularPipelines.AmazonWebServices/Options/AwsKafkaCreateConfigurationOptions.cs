using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "create-configuration")]
public record AwsKafkaCreateConfigurationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--server-properties")] string ServerProperties
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--kafka-versions")]
    public string[]? KafkaVersions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}