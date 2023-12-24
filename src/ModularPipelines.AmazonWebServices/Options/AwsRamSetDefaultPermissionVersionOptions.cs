using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "set-default-permission-version")]
public record AwsRamSetDefaultPermissionVersionOptions(
[property: CommandSwitch("--permission-arn")] string PermissionArn,
[property: CommandSwitch("--permission-version")] int PermissionVersion
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}