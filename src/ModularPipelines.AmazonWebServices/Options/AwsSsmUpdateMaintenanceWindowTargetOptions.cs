using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-maintenance-window-target")]
public record AwsSsmUpdateMaintenanceWindowTargetOptions(
[property: CliOption("--window-id")] string WindowId,
[property: CliOption("--window-target-id")] string WindowTargetId
) : AwsOptions
{
    [CliOption("--targets")]
    public string[]? Targets { get; set; }

    [CliOption("--owner-information")]
    public string? OwnerInformation { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}