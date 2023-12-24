using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "update-account-password-policy")]
public record AwsIamUpdateAccountPasswordPolicyOptions : AwsOptions
{
    [CommandSwitch("--minimum-password-length")]
    public int? MinimumPasswordLength { get; set; }

    [CommandSwitch("--max-password-age")]
    public int? MaxPasswordAge { get; set; }

    [CommandSwitch("--password-reuse-prevention")]
    public int? PasswordReusePrevention { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}