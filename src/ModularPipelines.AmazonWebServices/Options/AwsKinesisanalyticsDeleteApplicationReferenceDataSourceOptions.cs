using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalytics", "delete-application-reference-data-source")]
public record AwsKinesisanalyticsDeleteApplicationReferenceDataSourceOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--current-application-version-id")] long CurrentApplicationVersionId,
[property: CommandSwitch("--reference-id")] string ReferenceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}