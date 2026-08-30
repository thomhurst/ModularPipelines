using Azure.Identity;
using Azure.ResourceManager;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.UnitTests;

public class AzureCommandTests : TestBase
{
    private static readonly ArmClient ArmClient = new(new DefaultAzureCredential());

    public class AzureCommandModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Tools.Azure.Az.Account.ListAsync(new AzAccountListOptions
            {
                All = true,
            }, new CommandExecutionOptions { InternalDryRun = true }, cancellationToken);
        }
    }

    public class AzureCommandModule2 : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Tools.Azure.Az.Account.ManagementGroup.ListAsync(new AzAccountManagementGroupListOptions(),
                new CommandExecutionOptions { InternalDryRun = true }, cancellationToken);
        }
    }

    [Test]
    public async Task Azure_Command_Is_Expected_Command()
    {
        var result = await await RunModule<AzureCommandModule>(RegisterArmClient);

        await Assert.That(result.ValueOrDefault!.CommandInput)
            .IsEqualTo("az account list --all");
    }

    [Test]
    public async Task Azure_Command_With_Sub_Command_Group_Is_Expected_Command()
    {
        var result = await await RunModule<AzureCommandModule2>(RegisterArmClient);
        await Assert.That(result.ValueOrDefault!.CommandInput)
            .IsEqualTo("az account management-group list");
    }

    [Test]
    public async Task Spark_Job_Arguments_Are_Valued_Options()
    {
        var arguments = BuildArguments(new AzSynapseSparkJobSubmitOptions
        {
            JobArguments = ["first=value", "second=value"],
        });

        await Assert.That(arguments).IsEquivalentTo(
            ["--arguments", "first=value", "second=value"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(typeof(AzSynapseSparkJobSubmitOptions).GetProperty(nameof(CommandLineToolOptions.Arguments))!.DeclaringType)
            .IsEqualTo(typeof(CommandLineToolOptions));
    }

    [Test]
    public async Task Cassandra_Command_Arguments_Are_Valued_Options()
    {
        var arguments = BuildArguments(new AzManagedCassandraClusterInvokeCommandOptions
        {
            ClusterArguments = ["first=value", "second=value"],
        });

        await Assert.That(arguments).IsEquivalentTo(
            ["--arguments", "first=value", "second=value"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(typeof(AzManagedCassandraClusterInvokeCommandOptions).GetProperty(nameof(CommandLineToolOptions.Arguments))!.DeclaringType)
            .IsEqualTo(typeof(CommandLineToolOptions));
    }

    [Test]
    public async Task Acr_Task_Identity_Values_Are_Grouped()
    {
        var arguments = BuildArguments(new AzAcrTaskCreateOptions("task", "registry")
        {
            AssignIdentityValue = ["[system]", "/subscriptions/example/identity"],
        });

        await Assert.That(arguments).IsEquivalentTo(
            ["--name", "task", "--registry", "registry", "--assign-identity", "[system]", "/subscriptions/example/identity"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Acr_Task_Schedules_Are_Repeated()
    {
        var arguments = BuildArguments(new AzAcrTaskCreateOptions("task", "registry")
        {
            ScheduleValues = ["daily:0 0 * * *", "weekly:0 0 * * 0"],
        });

        await Assert.That(arguments).IsEquivalentTo(
            ["--name", "task", "--registry", "registry", "--schedule", "daily:0 0 * * *", "--schedule", "weekly:0 0 * * 0"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Acr_Task_Git_Access_Token_Is_A_Value()
    {
        var arguments = BuildArguments(new AzAcrTaskCreateOptions("task", "registry")
        {
            GitAccessTokenValue = "token",
        });

        await Assert.That(arguments).IsEquivalentTo(
            ["--name", "task", "--registry", "registry", "--git-access-token", "token"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Stack_Child_Scope_Option_Is_A_Flag()
    {
        var arguments = BuildArguments(new AzStackGroupCreateOptions("deleteAll", "none", "stack", "group")
        {
            Cs = true,
        });

        await Assert.That(arguments).IsEquivalentTo(
            ["--action-on-unmanage", "deleteAll", "--deny-settings-mode", "none", "--name", "stack", "--resource-group", "group", "--cs"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Stack_Whatif_Child_Scope_Option_Is_A_Flag()
    {
        var arguments = BuildArguments(new AzStackWhatifGroupCreateOptions(
            "deleteAll",
            "none",
            "stack",
            "group",
            "P7D",
            "stack-id")
        {
            Cs = true,
        });

        await Assert.That(arguments).IsEquivalentTo(
            [
                "--action-on-unmanage", "deleteAll",
                "--deny-settings-mode", "none",
                "--name", "stack",
                "--resource-group", "group",
                "--retention-interval", "P7D",
                "--stack-id", "stack-id",
                "--cs",
            ],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Expanded_Boolean_Option_Renders_Its_Value()
    {
        var arguments = BuildArguments(new AzMonitorAccountIssueUpdateOptions
        {
            ForceString = true,
        });

        await Assert.That(arguments).IsEquivalentTo(
            ["--force-string", "true"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    private static void RegisterArmClient(IServiceCollection services)
    {
        services.AddSingleton(ArmClient);
    }
}
