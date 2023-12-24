using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "sign")]
public record AwsCloudfrontSignOptions(
[property: CommandSwitch("--url")] string Url,
[property: CommandSwitch("--key-pair-id")] string KeyPairId,
[property: CommandSwitch("--private-key")] string PrivateKey,
[property: CommandSwitch("--date-less-than")] string DateLessThan
) : AwsOptions
{
    [CommandSwitch("--date-greater-than")]
    public string? DateGreaterThan { get; set; }

    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }
}