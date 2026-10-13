using ModularPipelines.Azure.Options;
using ModularPipelines.TestHelpers;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.UnitTests;

public class AzureGlobalArgumentsTests : TestBase
{
    [Test]
    public async Task Globals_Are_Inherited_And_Rendered_Once()
    {
        var arguments = BuildArguments(new AzGroupShowOptions("group")
        {
            Subscription = "subscription",
            Output = "json",
            Query = "name",
            Debug = true,
            OnlyShowErrors = true,
            Verbose = true,
            AcquirePolicyToken = true,
            ChangeReference = "change",
        });
        await Assert.That(arguments.Count(argument => argument == "--subscription")).IsEqualTo(1);
        await Assert.That(arguments.Count(argument => argument == "--output")).IsEqualTo(1);
        await Assert.That(arguments).Contains("subscription");
        await Assert.That(arguments).Contains("json");
        await Assert.That(arguments).Contains("--query");
        await Assert.That(arguments).Contains("--debug");
        await Assert.That(arguments).Contains("--only-show-errors");
        await Assert.That(arguments).Contains("--verbose");
        await Assert.That(arguments).Contains("--acquire-policy-token");
        await Assert.That(arguments).Contains("--change-reference");
        await Assert.That(typeof(AzGroupShowOptions).GetProperty(nameof(AzOptions.Subscription))!.DeclaringType)
            .IsEqualTo(typeof(AzOptions));
    }

    [Test]
    public async Task Required_Local_Subscription_Replaces_Inherited_Subscription()
    {
        var options = new AzAccountSetOptions("local");
        ((AzOptions) options).Subscription = "inherited";
        var arguments = BuildArguments(options);
        await Assert.That(arguments).IsEquivalentTo(["--name", "local"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Local_Subscription_Alias_Shares_Inherited_Storage()
    {
        var options = new AzAccountShowOptions { Subscription = "local" };
        ((AzOptions) options).Subscription = "inherited";
        await Assert.That(options.Subscription).IsEqualTo("inherited");
        await Assert.That(BuildArguments(options)).IsEquivalentTo(["--name", "inherited"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Globals_Follow_The_Complete_Subcommand_Path()
    {
        var command = await RenderCommand(new AzAccountManagementGroupListOptions
        {
            Subscription = "subscription",
            Output = "json",
        });
        await Assert.That(command)
            .IsEqualTo("az account management-group list --output json --subscription subscription");
    }
}
