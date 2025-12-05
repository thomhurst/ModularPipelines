using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "update-ip-set")]
public record AwsWafUpdateIpSetOptions(
[property: CliOption("--ip-set-id")] string IpSetId,
[property: CliOption("--change-token")] string ChangeToken,
[property: CliOption("--updates")] string[] Updates
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}