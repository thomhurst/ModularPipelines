using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appintegrations", "create-application")]
public record AwsAppintegrationsCreateApplicationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--application-source-config")] string ApplicationSourceConfig
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--subscriptions")]
    public string[]? Subscriptions { get; set; }

    [CliOption("--publications")]
    public string[]? Publications { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--permissions")]
    public string[]? Permissions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}