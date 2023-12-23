using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mq", "create-configuration")]
public record AwsMqCreateConfigurationOptions(
[property: CommandSwitch("--engine-type")] string EngineType,
[property: CommandSwitch("--engine-version")] string EngineVersion,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--authentication-strategy")]
    public string? AuthenticationStrategy { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}