using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "delete-kx-dataview")]
public record AwsFinspaceDeleteKxDataviewOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--dataview-name")] string DataviewName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}