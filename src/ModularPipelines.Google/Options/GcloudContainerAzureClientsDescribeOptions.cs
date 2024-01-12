using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "azure", "clients", "describe")]
public record GcloudContainerAzureClientsDescribeOptions(
[property: PositionalArgument] string Client,
[property: PositionalArgument] string Location
) : GcloudOptions;