using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "change-password")]
public record AwsIamChangePasswordOptions(
[property: CliOption("--old-password")] string OldPassword,
[property: CliOption("--new-password")] string NewPassword
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}