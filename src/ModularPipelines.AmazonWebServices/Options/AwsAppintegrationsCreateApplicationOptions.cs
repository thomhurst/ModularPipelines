using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appintegrations", "create-application")]
public record AwsAppintegrationsCreateApplicationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace")] string Namespace,
[property: CommandSwitch("--application-source-config")] string ApplicationSourceConfig
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--subscriptions")]
    public string[]? Subscriptions { get; set; }

    [CommandSwitch("--publications")]
    public string[]? Publications { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--permissions")]
    public string[]? Permissions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}