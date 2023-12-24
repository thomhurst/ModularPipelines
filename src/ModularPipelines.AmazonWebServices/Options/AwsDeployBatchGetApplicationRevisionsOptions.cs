using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "batch-get-application-revisions")]
public record AwsDeployBatchGetApplicationRevisionsOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--revisions")] string[] Revisions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}