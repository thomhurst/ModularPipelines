using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "create-permission")]
public record AwsRamCreatePermissionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--policy-template")] string PolicyTemplate
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}