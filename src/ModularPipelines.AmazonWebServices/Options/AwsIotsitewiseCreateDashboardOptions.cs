using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "create-dashboard")]
public record AwsIotsitewiseCreateDashboardOptions(
[property: CliOption("--project-id")] string ProjectId,
[property: CliOption("--dashboard-name")] string DashboardName,
[property: CliOption("--dashboard-definition")] string DashboardDefinition
) : AwsOptions
{
    [CliOption("--dashboard-description")]
    public string? DashboardDescription { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}