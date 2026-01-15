using ModularPipelines.Azure.Extensions;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class AzureCommandTests : TestBase
{
    public class AzureCommandModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Azure().Az.Account.List(new AzAccountListOptions
            {
                All = true,
            }, new CommandExecutionOptions { InternalDryRun = true }, cancellationToken);
        }
    }

    public class AzureCommandModule2 : Module<CommandResult>
    {
        protected internal override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Azure().Az.Account.ManagementGroup.List(new AzAccountManagementGroupListOptions(),
                new CommandExecutionOptions { InternalDryRun = true }, cancellationToken);
        }
    }

    [Test]
    public async Task Azure_Command_Is_Expected_Command()
    {
        var result = await await RunModule<AzureCommandModule>();

        await Assert.That(result.ValueOrDefault!.CommandInput)
            .IsEqualTo("az account list --all");
    }

    [Test]
    public async Task Azure_Command_With_Sub_Command_Group_Is_Expected_Command()
    {
        var result = await await RunModule<AzureCommandModule2>();
        await Assert.That(result.ValueOrDefault!.CommandInput)
            .IsEqualTo("az account management-group list");
    }
}