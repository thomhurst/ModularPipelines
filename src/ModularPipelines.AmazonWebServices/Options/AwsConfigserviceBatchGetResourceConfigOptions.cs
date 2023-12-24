using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "batch-get-resource-config")]
public record AwsConfigserviceBatchGetResourceConfigOptions(
[property: CommandSwitch("--resource-keys")] string[] ResourceKeys
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}