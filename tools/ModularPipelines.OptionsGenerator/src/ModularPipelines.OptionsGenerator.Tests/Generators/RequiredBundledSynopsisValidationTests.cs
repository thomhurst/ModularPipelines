using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments("(TARGET --mode=MODE | --global)", false)]
    [Arguments("(--global | TARGET --mode=MODE)", false)]
    [Arguments("(TARGET --mode=MODE | --global --region=REGION)", true)]
    [Arguments("(TARGET --mode=MODE | --global)", false, "TARGET")]
    [Arguments("(TARGET --mode=MODE | --global)", false, "--mode=MODE")]
    [Arguments("(TARGET --mode=MODE | --global)", false, "TARGET --region=REGION")]
    [Arguments("(TARGET --mode=MODE | --global)", false, "[TARGET]")]
    [Arguments("(TARGET --mode=MODE | --global)", false, "(--mode=MODE | --region=REGION)")]
    [Arguments("(TARGET --mode=MODE | --global) --mode=MODE", false, "TARGET", true)]
    [Arguments("(TARGET --mode=MODE | --global)", false, "OBJECT")]
    public async Task Required_Bundled_Synopsis_Validates_Complete_Alternatives(
        string syntax, bool requiresRegion, string? alternateSyntax = null, bool requiresMode = false)
    {
        var alternateSynopsis = alternateSyntax is null ? "" : $"\n    gcloud example run {alternateSyntax}";
        var help = $$"""
            NAME
                gcloud example run - run an example
            SYNOPSIS
                gcloud example run {{syntax}}{{alternateSynopsis}}
            POSITIONAL ARGUMENTS
                 [TARGET]
                    The target.
            FLAGS
                 --mode=MODE
                    The mode.
                 --global
                    Select all targets.
                 --region=REGION
                    The region.
            """;
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("example run", help)).Single();
        await Assert.That(command.PositionalArguments.Single().IsRequired).IsFalse().Because(command.UsageSynopsis!);
        var generated = await Generate([.. command.Options], command.PositionalArguments, command.RequiredAlternativeGroups);
        var optionsType = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;

        for (var selection = 0; selection < 16; selection++)
        {
            var target = (selection & 1) != 0;
            var mode = (selection & 2) != 0;
            var global = (selection & 4) != 0;
            var region = (selection & 8) != 0;
            var instance = Activator.CreateInstance(optionsType)!;
            optionsType.GetProperty("Target")!.SetValue(instance, target ? "target" : null);
            optionsType.GetProperty("Mode")!.SetValue(instance, mode ? "mode" : null);
            optionsType.GetProperty("Global")!.SetValue(instance, global ? true : null);
            optionsType.GetProperty("Region")!.SetValue(instance, region ? "region" : null);
            var errors = new List<ValidationResult>();
            var alternateValid = IsCompleteAlternateSynopsis(alternateSyntax, target, mode, region);
            var primaryValid = IsCompletePrimarySynopsis(target, mode, global, region, requiresMode, requiresRegion);

            await Assert.That(Validator.TryValidateObject(instance, new(instance), errors, true))
                .IsEqualTo(alternateValid || primaryValid)
                .Because($"Selection {selection}: {string.Join("; ", errors)}");
        }
    }

    private static bool IsCompletePrimarySynopsis(
        bool target, bool mode, bool global, bool region, bool requiresMode, bool requiresRegion) =>
        (!requiresMode || mode) && ((target && mode) || (global && (!requiresRegion || region)));

    private static bool IsCompleteAlternateSynopsis(string? syntax, bool target, bool mode, bool region) => syntax switch
    {
        "TARGET" or "OBJECT" => target,
        "--mode=MODE" => mode,
        "TARGET --region=REGION" => target && region,
        "[TARGET]" => true,
        "(--mode=MODE | --region=REGION)" => mode || region,
        _ => false,
    };
}
