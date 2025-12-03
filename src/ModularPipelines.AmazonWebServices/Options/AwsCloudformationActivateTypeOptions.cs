using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "activate-type")]
public record AwsCloudformationActivateTypeOptions : AwsOptions
{
    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--public-type-arn")]
    public string? PublicTypeArn { get; set; }

    [CliOption("--publisher-id")]
    public string? PublisherId { get; set; }

    [CliOption("--type-name")]
    public string? TypeName { get; set; }

    [CliOption("--type-name-alias")]
    public string? TypeNameAlias { get; set; }

    [CliOption("--logging-config")]
    public string? LoggingConfig { get; set; }

    [CliOption("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CliOption("--version-bump")]
    public string? VersionBump { get; set; }

    [CliOption("--major-version")]
    public long? MajorVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}