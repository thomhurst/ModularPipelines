using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "remove-listener-certificates")]
public record AwsElbv2RemoveListenerCertificatesOptions(
[property: CliOption("--listener-arn")] string ListenerArn,
[property: CliOption("--certificates")] string[] Certificates
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}