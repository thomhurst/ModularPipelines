using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzBilling
{
    public AzBilling(
        AzBillingAccount account,
        AzBillingAgreement agreement,
        AzBillingBalance balance,
        AzBillingCustomer customer,
        AzBillingEnrollmentAccount enrollmentAccount,
        AzBillingInstruction instruction,
        AzBillingInvoice invoice,
        AzBillingPeriod period,
        AzBillingPermission permission,
        AzBillingPolicy policy,
        AzBillingProduct product,
        AzBillingProfile profile,
        AzBillingProperty property,
        AzBillingRoleAssignment roleAssignment,
        AzBillingRoleDefinition roleDefinition,
        AzBillingSubscription subscription,
        AzBillingTransaction transaction
    )
    {
        Account = account;
        Agreement = agreement;
        Balance = balance;
        Customer = customer;
        EnrollmentAccount = enrollmentAccount;
        Instruction = instruction;
        Invoice = invoice;
        Period = period;
        Permission = permission;
        Policy = policy;
        Product = product;
        Profile = profile;
        Property = property;
        RoleAssignment = roleAssignment;
        RoleDefinition = roleDefinition;
        Subscription = subscription;
        Transaction = transaction;
    }

    public AzBillingAccount Account { get; }

    public AzBillingAgreement Agreement { get; }

    public AzBillingBalance Balance { get; }

    public AzBillingCustomer Customer { get; }

    public AzBillingEnrollmentAccount EnrollmentAccount { get; }

    public AzBillingInstruction Instruction { get; }

    public AzBillingInvoice Invoice { get; }

    public AzBillingPeriod Period { get; }

    public AzBillingPermission Permission { get; }

    public AzBillingPolicy Policy { get; }

    public AzBillingProduct Product { get; }

    public AzBillingProfile Profile { get; }

    public AzBillingProperty Property { get; }

    public AzBillingRoleAssignment RoleAssignment { get; }

    public AzBillingRoleDefinition RoleDefinition { get; }

    public AzBillingSubscription Subscription { get; }

    public AzBillingTransaction Transaction { get; }
}