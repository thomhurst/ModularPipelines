using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "create-permission-version")]
public record AwsRamCreatePermissionVersionOptions(
[property: CommandSwitch("--permission-arn")] string PermissionArn,
[property: CommandSwitch("--policy-template")] string PolicyTemplate
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}