using ModularPipelines.Azure.Extensions;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests;

public class AzureCommandTests : TestBase
{
    public class AzureCommandModule : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await context.Azure().Az.Account.Alias.Create(new AzAccountAliasCreateOptions("MyName")
            {
                InternalDryRun = true
            }, cancellationToken);
        }
    }

    public class AzureCommandModule2 : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await context.Azure().Az.Account.ManagementGroup.Subscription.Add(new AzAccountManagementGroupSubscriptionAddOptions("MyName", "MySub")
            {
                InternalDryRun = true
            }, cancellationToken);
        }
    }

    [Test]
    public async Task Azure_Command_Is_Expected_Command()
    {
        var result = await RunModule<AzureCommandModule>();
        
        await Assert.That(result.Result.Value!.CommandInput)
            .IsEqualTo("az account alias create --name MyName");
    }

    [Test]
    public async Task Azure_Command_With_Mandatory_Switch_Conflicting_With_Base_Default_Optional_Switch_Is_Expected_Command()
    {
        var result = await RunModule<AzureCommandModule2>();
        await Assert.That(result.Result.Value!.CommandInput)
            .IsEqualTo("az account management-group subscription add --name MyName --subscription MySub");
    }
}