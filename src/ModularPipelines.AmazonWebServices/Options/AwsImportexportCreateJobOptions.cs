using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("importexport", "create-job")]
public record AwsImportexportCreateJobOptions(
[property: CommandSwitch("--job-type")] string JobType,
[property: CommandSwitch("--manifest")] string Manifest
) : AwsOptions
{
    [CommandSwitch("--manifest-addendum")]
    public string? ManifestAddendum { get; set; }

    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}