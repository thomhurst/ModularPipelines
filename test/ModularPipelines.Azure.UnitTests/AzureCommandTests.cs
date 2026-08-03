using Azure.Identity;
using Azure.ResourceManager;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Azure.Extensions;
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
            return await context.Azure().Az.Account.ListAsync(new AzAccountListOptions
            {
                All = true,
            }, new CommandExecutionOptions { InternalDryRun = true }, cancellationToken);
        }
    }

    public class AzureCommandModule2 : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Azure().Az.Account.ManagementGroup.ListAsync(new AzAccountManagementGroupListOptions(),
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
            ["--arguments", "first=value", "--arguments", "second=value"],
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
            ["--arguments", "first=value", "--arguments", "second=value"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(typeof(AzManagedCassandraClusterInvokeCommandOptions).GetProperty(nameof(CommandLineToolOptions.Arguments))!.DeclaringType)
            .IsEqualTo(typeof(CommandLineToolOptions));
    }

    private static void RegisterArmClient(IServiceCollection services)
    {
        services.AddSingleton(ArmClient);
    }
}
