using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "create-app")]
public record AwsOpsworksCreateAppOptions(
[property: CommandSwitch("--stack-id")] string StackId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--shortname")]
    public string? Shortname { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--data-sources")]
    public string[]? DataSources { get; set; }

    [CommandSwitch("--app-source")]
    public string? AppSource { get; set; }

    [CommandSwitch("--domains")]
    public string[]? Domains { get; set; }

    [CommandSwitch("--ssl-configuration")]
    public string? SslConfiguration { get; set; }

    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--environment")]
    public string[]? Environment { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}