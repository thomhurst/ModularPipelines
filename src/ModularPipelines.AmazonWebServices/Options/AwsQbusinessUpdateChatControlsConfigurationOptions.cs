using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "update-chat-controls-configuration")]
public record AwsQbusinessUpdateChatControlsConfigurationOptions(
[property: CliOption("--application-id")] string ApplicationId
) : AwsOptions
{
    [CliOption("--blocked-phrases-configuration-update")]
    public string? BlockedPhrasesConfigurationUpdate { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--response-scope")]
    public string? ResponseScope { get; set; }

    [CliOption("--topic-configurations-to-create-or-update")]
    public string[]? TopicConfigurationsToCreateOrUpdate { get; set; }

    [CliOption("--topic-configurations-to-delete")]
    public string[]? TopicConfigurationsToDelete { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}