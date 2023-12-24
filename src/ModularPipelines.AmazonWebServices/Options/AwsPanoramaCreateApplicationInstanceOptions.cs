using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("panorama", "create-application-instance")]
public record AwsPanoramaCreateApplicationInstanceOptions(
[property: CommandSwitch("--default-runtime-context-device")] string DefaultRuntimeContextDevice,
[property: CommandSwitch("--manifest-payload")] string ManifestPayload
) : AwsOptions
{
    [CommandSwitch("--application-instance-id-to-replace")]
    public string? ApplicationInstanceIdToReplace { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--manifest-overrides-payload")]
    public string? ManifestOverridesPayload { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--runtime-role-arn")]
    public string? RuntimeRoleArn { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}