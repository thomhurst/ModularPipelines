using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-workforce")]
public record AwsSagemakerCreateWorkforceOptions(
[property: CommandSwitch("--workforce-name")] string WorkforceName
) : AwsOptions
{
    [CommandSwitch("--cognito-config")]
    public string? CognitoConfig { get; set; }

    [CommandSwitch("--oidc-config")]
    public string? OidcConfig { get; set; }

    [CommandSwitch("--source-ip-config")]
    public string? SourceIpConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--workforce-vpc-config")]
    public string? WorkforceVpcConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}