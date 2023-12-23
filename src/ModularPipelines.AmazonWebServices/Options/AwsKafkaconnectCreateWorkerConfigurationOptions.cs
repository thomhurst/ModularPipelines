using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafkaconnect", "create-worker-configuration")]
public record AwsKafkaconnectCreateWorkerConfigurationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--properties-file-content")] string PropertiesFileContent
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}