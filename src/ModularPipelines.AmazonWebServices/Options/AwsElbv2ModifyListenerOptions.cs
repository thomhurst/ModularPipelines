using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "modify-listener")]
public record AwsElbv2ModifyListenerOptions(
[property: CommandSwitch("--listener-arn")] string ListenerArn
) : AwsOptions
{
    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--ssl-policy")]
    public string? SslPolicy { get; set; }

    [CommandSwitch("--certificates")]
    public string[]? Certificates { get; set; }

    [CommandSwitch("--default-actions")]
    public string[]? DefaultActions { get; set; }

    [CommandSwitch("--alpn-policy")]
    public string[]? AlpnPolicy { get; set; }

    [CommandSwitch("--mutual-authentication")]
    public string? MutualAuthentication { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}