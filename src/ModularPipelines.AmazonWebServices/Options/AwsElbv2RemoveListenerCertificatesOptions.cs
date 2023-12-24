using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "remove-listener-certificates")]
public record AwsElbv2RemoveListenerCertificatesOptions(
[property: CommandSwitch("--listener-arn")] string ListenerArn,
[property: CommandSwitch("--certificates")] string[] Certificates
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}