using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "update-key")]
public record AwsLocationUpdateKeyOptions(
[property: CommandSwitch("--key-name")] string KeyName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--expire-time")]
    public long? ExpireTime { get; set; }

    [CommandSwitch("--restrictions")]
    public string? Restrictions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}