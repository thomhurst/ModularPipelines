using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "deployments", "describe")]
public record GcloudApigeeDeploymentsDescribeOptions : GcloudOptions;