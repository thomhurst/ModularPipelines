using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "sign")]
public record AwsCloudfrontSignOptions(
[property: CliOption("--url")] string Url,
[property: CliOption("--key-pair-id")] string KeyPairId,
[property: CliOption("--private-key")] string PrivateKey,
[property: CliOption("--date-less-than")] string DateLessThan
) : AwsOptions
{
    [CliOption("--date-greater-than")]
    public string? DateGreaterThan { get; set; }

    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }
}