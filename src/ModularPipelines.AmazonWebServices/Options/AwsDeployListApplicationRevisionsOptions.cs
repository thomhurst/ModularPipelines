using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "list-application-revisions")]
public record AwsDeployListApplicationRevisionsOptions(
[property: CommandSwitch("--application-name")] string ApplicationName
) : AwsOptions
{
    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--sort-order")]
    public string? SortOrder { get; set; }

    [CommandSwitch("--s3-bucket")]
    public string? S3Bucket { get; set; }

    [CommandSwitch("--s3-key-prefix")]
    public string? S3KeyPrefix { get; set; }

    [CommandSwitch("--deployed")]
    public string? Deployed { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}