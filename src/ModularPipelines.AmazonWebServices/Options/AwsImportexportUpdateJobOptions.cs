using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("importexport", "update-job")]
public record AwsImportexportUpdateJobOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--manifest")] string Manifest,
[property: CommandSwitch("--job-type")] string JobType
) : AwsOptions
{
    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}