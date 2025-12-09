using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "register-on-premises-instance")]
public record AwsDeployRegisterOnPremisesInstanceOptions(
[property: CliOption("--instance-name")] string InstanceName
) : AwsOptions
{
    [CliOption("--iam-session-arn")]
    public string? IamSessionArn { get; set; }

    [CliOption("--iam-user-arn")]
    public string? IamUserArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}