using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifybackend", "delete-token")]
public record AwsAmplifybackendDeleteTokenOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--session-id")] string SessionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}