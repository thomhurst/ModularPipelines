using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "delete-permission-version")]
public record AwsRamDeletePermissionVersionOptions(
[property: CommandSwitch("--permission-arn")] string PermissionArn,
[property: CommandSwitch("--permission-version")] int PermissionVersion
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}