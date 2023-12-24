using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "update-maintenance-window-target")]
public record AwsSsmUpdateMaintenanceWindowTargetOptions(
[property: CommandSwitch("--window-id")] string WindowId,
[property: CommandSwitch("--window-target-id")] string WindowTargetId
) : AwsOptions
{
    [CommandSwitch("--targets")]
    public string[]? Targets { get; set; }

    [CommandSwitch("--owner-information")]
    public string? OwnerInformation { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}