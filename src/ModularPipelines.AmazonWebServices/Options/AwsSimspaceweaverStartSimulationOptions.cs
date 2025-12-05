using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("simspaceweaver", "start-simulation")]
public record AwsSimspaceweaverStartSimulationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--maximum-duration")]
    public string? MaximumDuration { get; set; }

    [CliOption("--schema-s3-location")]
    public string? SchemaS3Location { get; set; }

    [CliOption("--snapshot-s3-location")]
    public string? SnapshotS3Location { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}