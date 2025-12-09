using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "create-project")]
public record AwsIotsitewiseCreateProjectOptions(
[property: CliOption("--portal-id")] string PortalId,
[property: CliOption("--project-name")] string ProjectName
) : AwsOptions
{
    [CliOption("--project-description")]
    public string? ProjectDescription { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}