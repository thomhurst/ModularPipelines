using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appintegrations", "update-application")]
public record AwsAppintegrationsUpdateApplicationOptions(
[property: CommandSwitch("--arn")] string Arn
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--application-source-config")]
    public string? ApplicationSourceConfig { get; set; }

    [CommandSwitch("--subscriptions")]
    public string[]? Subscriptions { get; set; }

    [CommandSwitch("--publications")]
    public string[]? Publications { get; set; }

    [CommandSwitch("--permissions")]
    public string[]? Permissions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}