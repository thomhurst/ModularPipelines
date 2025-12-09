using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "modify-listener")]
public record AwsElbv2ModifyListenerOptions(
[property: CliOption("--listener-arn")] string ListenerArn
) : AwsOptions
{
    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--ssl-policy")]
    public string? SslPolicy { get; set; }

    [CliOption("--certificates")]
    public string[]? Certificates { get; set; }

    [CliOption("--default-actions")]
    public string[]? DefaultActions { get; set; }

    [CliOption("--alpn-policy")]
    public string[]? AlpnPolicy { get; set; }

    [CliOption("--mutual-authentication")]
    public string? MutualAuthentication { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}