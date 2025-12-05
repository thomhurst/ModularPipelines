using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "register-target-with-maintenance-window")]
public record AwsSsmRegisterTargetWithMaintenanceWindowOptions(
[property: CliOption("--window-id")] string WindowId,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--targets")] string[] Targets
) : AwsOptions
{
    [CliOption("--owner-information")]
    public string? OwnerInformation { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}