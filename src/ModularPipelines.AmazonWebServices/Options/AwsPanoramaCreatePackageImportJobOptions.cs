using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("panorama", "create-package-import-job")]
public record AwsPanoramaCreatePackageImportJobOptions(
[property: CommandSwitch("--client-token")] string ClientToken,
[property: CommandSwitch("--input-config")] string InputConfig,
[property: CommandSwitch("--job-type")] string JobType,
[property: CommandSwitch("--output-config")] string OutputConfig
) : AwsOptions
{
    [CommandSwitch("--job-tags")]
    public string[]? JobTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}