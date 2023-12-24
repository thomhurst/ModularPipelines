using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("panorama", "create-node-from-template-job")]
public record AwsPanoramaCreateNodeFromTemplateJobOptions(
[property: CommandSwitch("--node-name")] string NodeName,
[property: CommandSwitch("--output-package-name")] string OutputPackageName,
[property: CommandSwitch("--output-package-version")] string OutputPackageVersion,
[property: CommandSwitch("--template-parameters")] IEnumerable<KeyValue> TemplateParameters,
[property: CommandSwitch("--template-type")] string TemplateType
) : AwsOptions
{
    [CommandSwitch("--job-tags")]
    public string[]? JobTags { get; set; }

    [CommandSwitch("--node-description")]
    public string? NodeDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}