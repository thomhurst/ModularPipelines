using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "describe-listener-certificates")]
public record AwsElbv2DescribeListenerCertificatesOptions(
[property: CommandSwitch("--listener-arn")] string ListenerArn
) : AwsOptions
{
    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}