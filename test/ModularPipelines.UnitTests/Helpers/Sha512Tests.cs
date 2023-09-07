﻿using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Helpers;

public class Sha512Tests : TestBase
{
    private class ToSha512Module : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha512("Foo bar!");
        }
    }

    [Test]
    public async Task To_Sha512_Has_Not_Errored()
    {
        var module = await RunModule<ToSha512Module>();

        var moduleResult = await module;

        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType, Is.EqualTo(ModuleResultType.Success));
            Assert.That(moduleResult.Exception, Is.Null);
            Assert.That(moduleResult.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task To_Sha512_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<ToSha512Module>();

        var moduleResult = await module;

        Assert.That(moduleResult.Value, Is.EqualTo("e399b0584705f5f229a4398baa31c4b7cc820ac208327d26e66f0668288536981c3460a7ea92ef6be488ce30ff5b6a991babfe24833094eba3226cea5c14162c"));
    }
}
