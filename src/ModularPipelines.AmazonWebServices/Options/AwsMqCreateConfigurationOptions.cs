using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mq", "create-configuration")]
public record AwsMqCreateConfigurationOptions(
[property: CliOption("--engine-type")] string EngineType,
[property: CliOption("--engine-version")] string EngineVersion,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--authentication-strategy")]
    public string? AuthenticationStrategy { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}