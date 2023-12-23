using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "change-password")]
public record AwsIamChangePasswordOptions(
[property: CommandSwitch("--old-password")] string OldPassword,
[property: CommandSwitch("--new-password")] string NewPassword
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}