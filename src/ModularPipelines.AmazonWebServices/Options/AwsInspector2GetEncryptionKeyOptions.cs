using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector2", "get-encryption-key")]
public record AwsInspector2GetEncryptionKeyOptions(
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--scan-type")] string ScanType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}