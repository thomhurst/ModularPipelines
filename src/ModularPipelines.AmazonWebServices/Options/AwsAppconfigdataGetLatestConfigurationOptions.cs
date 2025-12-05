using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfigdata", "get-latest-configuration")]
public record AwsAppconfigdataGetLatestConfigurationOptions(
[property: CliOption("--configuration-token")] string ConfigurationToken
) : AwsOptions;