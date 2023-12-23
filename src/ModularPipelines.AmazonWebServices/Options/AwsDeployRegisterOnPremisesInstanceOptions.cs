using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "register-on-premises-instance")]
public record AwsDeployRegisterOnPremisesInstanceOptions(
[property: CommandSwitch("--instance-name")] string InstanceName
) : AwsOptions
{
    [CommandSwitch("--iam-session-arn")]
    public string? IamSessionArn { get; set; }

    [CommandSwitch("--iam-user-arn")]
    public string? IamUserArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}