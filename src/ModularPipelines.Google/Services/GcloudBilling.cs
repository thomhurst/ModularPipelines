using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudBilling
{
    public GcloudBilling(
        GcloudBillingAccounts accounts,
        GcloudBillingBudgets budgets,
        GcloudBillingProjects projects
    )
    {
        Accounts = accounts;
        Budgets = budgets;
        Projects = projects;
    }

    public GcloudBillingAccounts Accounts { get; }

    public GcloudBillingBudgets Budgets { get; }

    public GcloudBillingProjects Projects { get; }
}