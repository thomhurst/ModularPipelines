using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-connections", "update-host")]
public record AwsCodestarConnectionsUpdateHostOptions(
[property: CommandSwitch("--host-arn")] string HostArn
) : AwsOptions
{
    [CommandSwitch("--provider-endpoint")]
    public string? ProviderEndpoint { get; set; }

    [CommandSwitch("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}