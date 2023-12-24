using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "create-plugin")]
public record AwsQbusinessCreatePluginOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--auth-configuration")] string AuthConfiguration,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--server-url")] string ServerUrl,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}