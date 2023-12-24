using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "create-listener")]
public record AwsElbv2CreateListenerOptions(
[property: CommandSwitch("--load-balancer-arn")] string LoadBalancerArn,
[property: CommandSwitch("--default-actions")] string[] DefaultActions
) : AwsOptions
{
    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--ssl-policy")]
    public string? SslPolicy { get; set; }

    [CommandSwitch("--certificates")]
    public string[]? Certificates { get; set; }

    [CommandSwitch("--alpn-policy")]
    public string[]? AlpnPolicy { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--mutual-authentication")]
    public string? MutualAuthentication { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}