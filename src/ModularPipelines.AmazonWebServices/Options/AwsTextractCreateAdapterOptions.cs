using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("textract", "create-adapter")]
public record AwsTextractCreateAdapterOptions(
[property: CliOption("--adapter-name")] string AdapterName,
[property: CliOption("--feature-types")] string[] FeatureTypes
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--auto-update")]
    public string? AutoUpdate { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}