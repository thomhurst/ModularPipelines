using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzAutomation
{
    public AzAutomation(
        AzAutomationAccount account,
        AzAutomationHrwg hrwg,
        AzAutomationJob job,
        AzAutomationPython3Package python3Package,
        AzAutomationRunbook runbook,
        AzAutomationSchedule schedule,
        AzAutomationSoftwareUpdateConfiguration softwareUpdateConfiguration
    )
    {
        Account = account;
        Hrwg = hrwg;
        Job = job;
        Python3Package = python3Package;
        Runbook = runbook;
        Schedule = schedule;
        SoftwareUpdateConfiguration = softwareUpdateConfiguration;
    }

    public AzAutomationAccount Account { get; }

    public AzAutomationHrwg Hrwg { get; }

    public AzAutomationJob Job { get; }

    public AzAutomationPython3Package Python3Package { get; }

    public AzAutomationRunbook Runbook { get; }

    public AzAutomationSchedule Schedule { get; }

    public AzAutomationSoftwareUpdateConfiguration SoftwareUpdateConfiguration { get; }
}