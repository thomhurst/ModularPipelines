﻿using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Helpers;

public class HexTests : TestBase
{
    private class ToHexModule : Module<string>
    {
        protected override async Task<ModuleResult<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hex.ToHex("Foo bar!");
        }
    }

    [Test]
    public async Task To_Hex_Has_Not_Errored()
    {
        var module = await RunModule<ToHexModule>();

        var moduleResult = await module;
        
        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType, Is.EqualTo(ModuleResultType.SuccessfulResult));
            Assert.That(moduleResult.Exception, Is.Null);
            Assert.That(moduleResult.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task To_Hex_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<ToHexModule>();

        var moduleResult = await module;
        
        Assert.That(moduleResult.Value, Is.EqualTo("466f6f2062617221"));
    }
    
    private class FromHexModule : Module<string>
    {
        protected override async Task<ModuleResult<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hex.FromHex("466f6f2062617221");
        }
    }

    [Test]
    public async Task From_Hex_Has_Not_Errored()
    {
        var module = await RunModule<FromHexModule>();

        var moduleResult = await module;
        
        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType, Is.EqualTo(ModuleResultType.SuccessfulResult));
            Assert.That(moduleResult.Exception, Is.Null);
            Assert.That(moduleResult.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task From_Hex_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<FromHexModule>();

        var moduleResult = await module;
        
        Assert.That(moduleResult.Value, Is.EqualTo("Foo bar!"));
    }
}