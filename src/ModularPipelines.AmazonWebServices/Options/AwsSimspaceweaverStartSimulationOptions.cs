using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("simspaceweaver", "start-simulation")]
public record AwsSimspaceweaverStartSimulationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--maximum-duration")]
    public string? MaximumDuration { get; set; }

    [CommandSwitch("--schema-s3-location")]
    public string? SchemaS3Location { get; set; }

    [CommandSwitch("--snapshot-s3-location")]
    public string? SnapshotS3Location { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}