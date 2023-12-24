using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sts", "assume-role")]
public record AwsStsAssumeRoleOptions(
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--role-session-name")] string RoleSessionName
) : AwsOptions
{
    [CommandSwitch("--policy-arns")]
    public string[]? PolicyArns { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--transitive-tag-keys")]
    public string[]? TransitiveTagKeys { get; set; }

    [CommandSwitch("--external-id")]
    public string? ExternalId { get; set; }

    [CommandSwitch("--serial-number")]
    public string? SerialNumber { get; set; }

    [CommandSwitch("--token-code")]
    public string? TokenCode { get; set; }

    [CommandSwitch("--source-identity")]
    public string? SourceIdentity { get; set; }

    [CommandSwitch("--provided-contexts")]
    public string[]? ProvidedContexts { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}