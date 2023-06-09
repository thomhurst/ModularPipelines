﻿using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Helpers;

public class Base64Tests : TestBase
{
    private class ToBase64Module : Module<string>
    {
        protected override async Task<ModuleResult<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Base64.ToBase64String("Foo bar!");
        }
    }

    [Test]
    public async Task To_Base64_Has_Not_Errored()
    {
        var module = await RunModule<ToBase64Module>();

        var moduleResult = await module;

        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType, Is.EqualTo(ModuleResultType.SuccessfulResult));
            Assert.That(moduleResult.Exception, Is.Null);
            Assert.That(moduleResult.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task To_Base64_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<ToBase64Module>();

        var moduleResult = await module;

        Assert.That(moduleResult.Value, Is.EqualTo("Rm9vIGJhciE="));
    }

    private class FromBase64Module : Module<string>
    {
        protected override async Task<ModuleResult<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Base64.FromBase64String("Rm9vIGJhciE=");
        }
    }

    [Test]
    public async Task From_Base64_Has_Not_Errored()
    {
        var module = await RunModule<FromBase64Module>();

        var moduleResult = await module;

        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType, Is.EqualTo(ModuleResultType.SuccessfulResult));
            Assert.That(moduleResult.Exception, Is.Null);
            Assert.That(moduleResult.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task From_Base64_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<FromBase64Module>();

        var moduleResult = await module;

        Assert.That(moduleResult.Value, Is.EqualTo("Foo bar!"));
    }
}
