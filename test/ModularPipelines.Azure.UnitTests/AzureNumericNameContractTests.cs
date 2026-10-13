using ModularPipelines.Azure.Options;
using ModularPipelines.Models;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Azure.UnitTests;

public class AzureNumericNameContractTests
{
    [Test]
    [Arguments(typeof(AzStorageAccountBlobServicePropertiesUpdateOptions), "EnableStaticWebsite", "--enable-static-website", "account")]
    [Arguments(typeof(AzStorageBlobServicePropertiesUpdateOptions), "StaticWebsite", "--static-website", null)]
    [Arguments(typeof(AzStorageFsServicePropertiesUpdateOptions), "StaticWebsite", "--static-website", null)]
    public async Task Error_Document_Name_Preserves_Switch_And_Static_Website_Option(
        Type optionsType,
        string staticWebsiteProperty,
        string staticWebsiteSwitch,
        string? accountName)
    {
        var options = Activator.CreateInstance(optionsType, accountName is null ? [] : [accountName])!;
        optionsType.GetProperty("Number404Document")!.SetValue(options, "error.html");

        var staticWebsite = optionsType.GetProperty(staticWebsiteProperty)!;
        await Assert.That(staticWebsite).IsNotNull();
        staticWebsite.SetValue(options, (CliOptionValue) "true");
        string[] expected = accountName is null
            ? ["--404-document", "error.html", staticWebsiteSwitch, "true"]
            : ["--account-name", accountName, "--404-document", "error.html", staticWebsiteSwitch, "true"];
        await Assert.That(BuildArguments(options)).IsEquivalentTo(expected);
    }

    [Test]
    [Arguments(typeof(AzFunctionappConfigSetOptions))]
    [Arguments(typeof(AzWebappConfigSetOptions))]
    public async Task Numeric_Name_Segment_Preserves_Worker_Process_Switch(Type optionsType)
    {
        var options = Activator.CreateInstance(optionsType)!;
        optionsType.GetProperty("UseNumber32bitWorkerProcess")!.SetValue(options, (CliOptionValue) "true");

        await Assert.That(BuildArguments(options)).IsEquivalentTo(["--use-32bit-worker-process", "true"]);
    }
}
