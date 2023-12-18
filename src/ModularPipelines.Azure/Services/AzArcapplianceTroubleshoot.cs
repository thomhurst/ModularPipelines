using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance")]
public class AzArcapplianceTroubleshoot
{
    public AzArcapplianceTroubleshoot(
        AzArcapplianceTroubleshootCommand command
    )
    {
        Command = command;
    }

    public AzArcapplianceTroubleshootCommand Command { get; }
}