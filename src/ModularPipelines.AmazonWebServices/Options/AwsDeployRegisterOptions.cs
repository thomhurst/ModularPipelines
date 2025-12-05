using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "register")]
public record AwsDeployRegisterOptions(
[property: CliOption("--instance-name")] string InstanceName
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--iam-user-arn")]
    public string? IamUserArn { get; set; }
}