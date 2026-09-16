using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    public async Task Captured_Certificate_Revocation_Validates_Alternatives_At_Runtime()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,
            "Fixtures", "Gcloud", "585.0.0", "gcloud-privateca-certificates-revoke.txt"));
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("privateca certificates revoke", help)).Single();
        await Assert.That(command.Options.Any(option => option.IsRequired)).IsFalse();
        await Assert.That(command.Options.Single(option => option.PropertyName == "SerialNumber").CSharpType)
            .IsEqualTo("string?");
        var tool = CreateGcloudValidationTool(command);
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        var enums = (await new EnumGenerator().GenerateAsync(tool)).Select(file => file.Content).ToArray();
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            const string serial = "7dc1d9186372de2e1f4824abb1c4c9e5e43cbb40";
            foreach (var (certificate, number, pool, location, valid) in new (string?, string?, string?, string?, bool)[]
            {
                (null, null, null, null, false),
                (" ", "", null, null, false),
                ("certificate", null, null, null, true),
                (null, serial, null, null, true),
                ("certificate", serial, null, null, false),
                (null, serial, "pool", "location", true),
                ("certificate", null, "pool", null, true),
                (null, serial, null, "location", false),
            })
            {
                var instance = Activator.CreateInstance(type)!;
                type.GetProperty("Certificate")!.SetValue(instance, certificate);
                type.GetProperty("SerialNumber")!.SetValue(instance, number);
                type.GetProperty("IssuerPool")!.SetValue(instance, pool);
                type.GetProperty("IssuerLocation")!.SetValue(instance, location);
                var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
                await Assert.That(errors.Length == 0).IsEqualTo(valid)
                    .Because($"certificate={certificate}, serial={number}, pool={pool}, location={location}: "
                        + string.Join("; ", errors.Select(error => error.ErrorMessage)));
            }
        }, enums);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Integer_Group_Members_Compile_And_Treat_Zero_As_Present(bool required)
    {
        var command = new CliCommandDefinition
        {
            FullCommand = "gcloud example count",
            CommandParts = ["example", "count"],
            ClassName = "GcloudExampleCountOptions",
            ParentClassName = "GcloudOptions",
            ToolNamespacePrefix = "Gcloud",
            Options = [new CliOptionDefinition
            {
                SwitchName = "--count", PropertyName = "Count", CSharpType = "int?", IsRequired = required,
            }],
            RequiredAlternativeGroups = [new CliRequiredAlternativeGroup
            {
                Members = [new CliRequiredAlternativeMember { PropertyName = "Count", OptionSwitch = "--count" }],
            }],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(CreateGcloudValidationTool(command))).Single().Content;
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            var instance = Activator.CreateInstance(type, required ? [0] : [])!;
            if (!required)
            {
                await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any()).IsTrue();
                type.GetProperty("Count")!.SetValue(instance, 0);
            }
            await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any()).IsFalse();
        });
    }

    private static CliToolDefinition CreateGcloudValidationTool(CliCommandDefinition command) => new()
    {
        ToolName = "gcloud",
        NamespacePrefix = "Gcloud",
        TargetNamespace = "ModularPipelines.Google",
        OutputDirectory = "output",
        Commands = [command],
    };
}
