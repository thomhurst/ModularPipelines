using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("panorama", "create-application-instance")]
public record AwsPanoramaCreateApplicationInstanceOptions(
[property: CliOption("--default-runtime-context-device")] string DefaultRuntimeContextDevice,
[property: CliOption("--manifest-payload")] string ManifestPayload
) : AwsOptions
{
    [CliOption("--application-instance-id-to-replace")]
    public string? ApplicationInstanceIdToReplace { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--manifest-overrides-payload")]
    public string? ManifestOverridesPayload { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--runtime-role-arn")]
    public string? RuntimeRoleArn { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}