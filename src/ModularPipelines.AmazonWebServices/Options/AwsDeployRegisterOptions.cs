using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "register")]
public record AwsDeployRegisterOptions(
[property: CommandSwitch("--instance-name")] string InstanceName
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--iam-user-arn")]
    public string? IamUserArn { get; set; }
}