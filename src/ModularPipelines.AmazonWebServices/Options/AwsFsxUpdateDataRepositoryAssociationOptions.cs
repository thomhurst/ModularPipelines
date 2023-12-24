using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "update-data-repository-association")]
public record AwsFsxUpdateDataRepositoryAssociationOptions(
[property: CommandSwitch("--association-id")] string AssociationId
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--imported-file-chunk-size")]
    public int? ImportedFileChunkSize { get; set; }

    [CommandSwitch("--s3")]
    public string? S3 { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}