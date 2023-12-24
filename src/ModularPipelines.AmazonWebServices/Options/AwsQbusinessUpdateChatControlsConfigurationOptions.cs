using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "update-chat-controls-configuration")]
public record AwsQbusinessUpdateChatControlsConfigurationOptions(
[property: CommandSwitch("--application-id")] string ApplicationId
) : AwsOptions
{
    [CommandSwitch("--blocked-phrases-configuration-update")]
    public string? BlockedPhrasesConfigurationUpdate { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--response-scope")]
    public string? ResponseScope { get; set; }

    [CommandSwitch("--topic-configurations-to-create-or-update")]
    public string[]? TopicConfigurationsToCreateOrUpdate { get; set; }

    [CommandSwitch("--topic-configurations-to-delete")]
    public string[]? TopicConfigurationsToDelete { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}