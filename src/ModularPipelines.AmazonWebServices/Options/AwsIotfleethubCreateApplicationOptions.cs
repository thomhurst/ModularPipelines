using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotfleethub", "create-application")]
public record AwsIotfleethubCreateApplicationOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--application-description")]
    public string? ApplicationDescription { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}