using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "register-target-with-maintenance-window")]
public record AwsSsmRegisterTargetWithMaintenanceWindowOptions(
[property: CommandSwitch("--window-id")] string WindowId,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--targets")] string[] Targets
) : AwsOptions
{
    [CommandSwitch("--owner-information")]
    public string? OwnerInformation { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}