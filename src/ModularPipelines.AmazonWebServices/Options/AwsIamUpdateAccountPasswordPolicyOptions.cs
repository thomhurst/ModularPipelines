using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "update-account-password-policy")]
public record AwsIamUpdateAccountPasswordPolicyOptions : AwsOptions
{
    [CliOption("--minimum-password-length")]
    public int? MinimumPasswordLength { get; set; }

    [CliOption("--max-password-age")]
    public int? MaxPasswordAge { get; set; }

    [CliOption("--password-reuse-prevention")]
    public int? PasswordReusePrevention { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}