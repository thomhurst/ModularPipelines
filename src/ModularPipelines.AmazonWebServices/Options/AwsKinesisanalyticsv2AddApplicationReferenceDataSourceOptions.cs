using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "add-application-reference-data-source")]
public record AwsKinesisanalyticsv2AddApplicationReferenceDataSourceOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--current-application-version-id")] long CurrentApplicationVersionId,
[property: CommandSwitch("--reference-data-source")] string ReferenceDataSource
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}