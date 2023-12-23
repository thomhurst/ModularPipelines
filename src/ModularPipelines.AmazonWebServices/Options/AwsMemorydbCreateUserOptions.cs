using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memorydb", "create-user")]
public record AwsMemorydbCreateUserOptions(
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--authentication-mode")] string AuthenticationMode,
[property: CommandSwitch("--access-string")] string AccessString
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}