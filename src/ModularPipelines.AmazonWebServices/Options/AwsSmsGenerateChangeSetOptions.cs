using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sms", "generate-change-set")]
public record AwsSmsGenerateChangeSetOptions : AwsOptions
{
    [CliOption("--app-id")]
    public string? AppId { get; set; }

    [CliOption("--changeset-format")]
    public string? ChangesetFormat { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}