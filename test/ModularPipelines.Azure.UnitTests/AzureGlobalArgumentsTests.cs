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
        await Assert.That(typeof(AzGroupShowOptions).GetProperty(nameof(AzGroupShowOptions.Subscription))!.DeclaringType)
            .IsEqualTo(typeof(AzGroupShowOptions));
        await Assert.That(typeof(AzGroupShowOptions).GetProperty(nameof(AzOptions.Output))!.DeclaringType)
            .IsEqualTo(typeof(AzOptions));
    }

    [Test]
    public async Task Account_Subscription_Alias_Uses_The_Parser_Primary_Name()
    {
        var options = new AzAccountSetOptions("local");
        var arguments = BuildArguments(options);
        await Assert.That(arguments).IsEquivalentTo(["--name", "local"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Optional_Account_Subscription_Alias_Uses_The_Parser_Primary_Name()
    {
        var options = new AzAccountShowOptions { Name = "local" };
        await Assert.That(BuildArguments(options)).IsEquivalentTo(["--name", "local"],
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Globals_Follow_The_Complete_Subcommand_Path()
    {
        var command = await RenderCommand(new AzAcrTokenListOptions("registry")
        {
            Subscription = "subscription",
            Output = "json",
        });
        await Assert.That(command)
            .IsEqualTo("az acr token list --registry registry --subscription subscription --output json");
    }

    [Test]
    public async Task Unsupported_Commands_Do_Not_Expose_Subscription()
    {
        await Assert.That(typeof(AzOptions).GetProperty("Subscription")).IsNull();
        await Assert.That(typeof(AzExtensionListOptions).GetProperty("Subscription")).IsNull();
        await Assert.That(await RenderCommand(new AzExtensionListOptions { Output = "json" }))
            .IsEqualTo("az extension list --output json");
    }

    [Test]
    public async Task Optional_Boolean_Controls_Do_Not_Mask_Their_Values()
    {
        await Assert.That(typeof(AzAcrTokenCreateOptions).GetProperty("NoPasswords")!
            .IsDefined(typeof(ModularPipelines.Secrets.SecretValueAttribute), inherit: true)).IsFalse();
        await Assert.That(typeof(AzPipelinesVariableCreateOptions).GetProperty("Secret")!
            .IsDefined(typeof(ModularPipelines.Secrets.SecretValueAttribute), inherit: true)).IsFalse();
    }
}
