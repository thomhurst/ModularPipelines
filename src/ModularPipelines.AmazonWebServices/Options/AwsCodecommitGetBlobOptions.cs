using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "get-blob")]
public record AwsCodecommitGetBlobOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--blob-id")] string BlobId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}