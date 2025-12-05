using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "azure", "clients", "describe")]
public record GcloudContainerAzureClientsDescribeOptions(
[property: CliArgument] string Client,
[property: CliArgument] string Location
) : GcloudOptions;