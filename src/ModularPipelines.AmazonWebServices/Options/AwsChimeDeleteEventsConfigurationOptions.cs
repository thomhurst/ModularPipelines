using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "delete-events-configuration")]
public record AwsChimeDeleteEventsConfigurationOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--bot-id")] string BotId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}