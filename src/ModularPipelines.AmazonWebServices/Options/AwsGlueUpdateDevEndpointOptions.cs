using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "update-dev-endpoint")]
public record AwsGlueUpdateDevEndpointOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName
) : AwsOptions
{
    [CommandSwitch("--public-key")]
    public string? PublicKey { get; set; }

    [CommandSwitch("--add-public-keys")]
    public string[]? AddPublicKeys { get; set; }

    [CommandSwitch("--delete-public-keys")]
    public string[]? DeletePublicKeys { get; set; }

    [CommandSwitch("--custom-libraries")]
    public string? CustomLibraries { get; set; }

    [CommandSwitch("--delete-arguments")]
    public string[]? DeleteArguments { get; set; }

    [CommandSwitch("--add-arguments")]
    public IEnumerable<KeyValue>? AddArguments { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}