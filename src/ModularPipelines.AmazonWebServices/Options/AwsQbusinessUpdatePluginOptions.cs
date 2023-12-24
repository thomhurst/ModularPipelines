using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "update-plugin")]
public record AwsQbusinessUpdatePluginOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--plugin-id")] string PluginId
) : AwsOptions
{
    [CommandSwitch("--auth-configuration")]
    public string? AuthConfiguration { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--server-url")]
    public string? ServerUrl { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}