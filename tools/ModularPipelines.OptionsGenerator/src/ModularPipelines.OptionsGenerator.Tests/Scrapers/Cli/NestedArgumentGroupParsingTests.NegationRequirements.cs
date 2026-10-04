using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Generators;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Gcloud_Named_Negatable_Trigger_Requires_Companion_Only_For_Emitted_Switch(bool negativeTrigger)
    {
        var help = $"""
            NAME
                gcloud example create - create an example
            FLAGS
                --[no-]confirm
                    Confirm the operation.
                --file=FILE
                    Required to be set when --{(negativeTrigger ? "no-" : "")}confirm is used.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        var generated = (await new OptionsClassGenerator().GenerateAsync(CreateGcloudValidationTool(command))).Single().Content;
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            foreach (var value in new bool?[] { null, false, true })
            {
                foreach (var withFile in new[] { false, true })
                {
                    var instance = Activator.CreateInstance(type)!;
                    type.GetProperty("Confirm")!.SetValue(instance, value);
                    type.GetProperty("File")!.SetValue(instance, withFile ? "input.json" : null);
                    var errors = instance is IValidatableObject validatable
                        ? validatable.Validate(new ValidationContext(instance)).ToArray()
                        : [];
                    await Assert.That(errors.Length == 0).IsEqualTo(withFile || value != !negativeTrigger)
                        .Because($"Confirm={value}, File={withFile}, negative trigger={negativeTrigger}");
                }
            }
        });
    }

    [Test]
    [Arguments("--no-confirm")]
    [Arguments("--confirm | --no-confirm")]
    public async Task Gcloud_Synopsis_Negation_Aliases_Preserve_Bundled_Alternative(string flagBranch)
    {
        var help = $"""
            NAME
                gcloud example create - create an example
            SYNOPSIS
                gcloud example create ({flagBranch} | --file=FILE --type=TYPE)
            FLAGS
                Exactly one of these must be specified:
                    --confirm
                        Confirm the operation. Use --no-confirm to disable.
                    --no-confirm
                        Disable confirmation.
                    --file=FILE
                        Select the file.
                    --type=TYPE
                        Select the type.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        var generated = (await new OptionsClassGenerator().GenerateAsync(CreateGcloudValidationTool(command))).Single().Content;
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            foreach (var confirm in new bool?[] { null, false, true })
            {
                for (var settings = 0; settings < 4; settings++)
                {
                    var instance = Activator.CreateInstance(type)!;
                    type.GetProperty("Confirm")!.SetValue(instance, confirm);
                    type.GetProperty("File")!.SetValue(instance, (settings & 1) != 0 ? "input.json" : null);
                    type.GetProperty("Type")!.SetValue(instance, (settings & 2) != 0 ? "resource" : null);
                    var errors = ((IValidatableObject) instance).Validate(new ValidationContext(instance));
                    await Assert.That(!errors.Any()).IsEqualTo(confirm.HasValue ? settings == 0 : settings != 0)
                        .Because($"Confirm={confirm}, settings={settings}");
                }
            }
        });
    }
}
