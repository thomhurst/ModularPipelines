using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pca-connector-ad", "create-template")]
public record AwsPcaConnectorAdCreateTemplateOptions(
[property: CommandSwitch("--connector-arn")] string ConnectorArn,
[property: CommandSwitch("--definition")] string Definition,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}