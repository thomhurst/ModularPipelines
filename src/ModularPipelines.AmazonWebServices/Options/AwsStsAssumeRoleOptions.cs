using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sts", "assume-role")]
public record AwsStsAssumeRoleOptions(
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--role-session-name")] string RoleSessionName
) : AwsOptions
{
    [CliOption("--policy-arns")]
    public string[]? PolicyArns { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--transitive-tag-keys")]
    public string[]? TransitiveTagKeys { get; set; }

    [CliOption("--external-id")]
    public string? ExternalId { get; set; }

    [CliOption("--serial-number")]
    public string? SerialNumber { get; set; }

    [CliOption("--token-code")]
    public string? TokenCode { get; set; }

    [CliOption("--source-identity")]
    public string? SourceIdentity { get; set; }

    [CliOption("--provided-contexts")]
    public string[]? ProvidedContexts { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}