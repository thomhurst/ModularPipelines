using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-connections", "update-host")]
public record AwsCodestarConnectionsUpdateHostOptions(
[property: CliOption("--host-arn")] string HostArn
) : AwsOptions
{
    [CliOption("--provider-endpoint")]
    public string? ProviderEndpoint { get; set; }

    [CliOption("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}