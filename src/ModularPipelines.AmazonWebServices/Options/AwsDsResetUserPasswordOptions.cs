using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "reset-user-password")]
public record AwsDsResetUserPasswordOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--new-password")] string NewPassword
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}