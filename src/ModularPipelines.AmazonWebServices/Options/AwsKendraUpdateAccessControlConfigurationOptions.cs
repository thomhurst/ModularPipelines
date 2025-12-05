using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "update-access-control-configuration")]
public record AwsKendraUpdateAccessControlConfigurationOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--access-control-list")]
    public string[]? AccessControlList { get; set; }

    [CliOption("--hierarchical-access-control-list")]
    public string[]? HierarchicalAccessControlList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}