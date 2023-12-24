using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-workforce")]
public record AwsSagemakerUpdateWorkforceOptions(
[property: CommandSwitch("--workforce-name")] string WorkforceName
) : AwsOptions
{
    [CommandSwitch("--source-ip-config")]
    public string? SourceIpConfig { get; set; }

    [CommandSwitch("--oidc-config")]
    public string? OidcConfig { get; set; }

    [CommandSwitch("--workforce-vpc-config")]
    public string? WorkforceVpcConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}