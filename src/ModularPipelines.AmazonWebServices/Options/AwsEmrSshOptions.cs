using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "ssh")]
public record AwsEmrSshOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--key-pair-file")] string KeyPairFile
) : AwsOptions
{
    [CommandSwitch("--command")]
    public string? Command { get; set; }
}