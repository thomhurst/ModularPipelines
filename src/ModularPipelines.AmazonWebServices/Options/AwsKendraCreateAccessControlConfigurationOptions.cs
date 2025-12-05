using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "create-access-control-configuration")]
public record AwsKendraCreateAccessControlConfigurationOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--access-control-list")]
    public string[]? AccessControlList { get; set; }

    [CliOption("--hierarchical-access-control-list")]
    public string[]? HierarchicalAccessControlList { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}