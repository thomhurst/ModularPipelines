using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplify", "generate-access-logs")]
public record AwsAmplifyGenerateAccessLogsOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--app-id")] string AppId
) : AwsOptions
{
    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}