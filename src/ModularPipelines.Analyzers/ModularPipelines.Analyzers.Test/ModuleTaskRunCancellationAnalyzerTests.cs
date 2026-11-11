using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleTaskRunCancellationAnalyzerTests
{
    [TestMethod]
    [DataRow("token", true)]
    [DataRow("", false)]
    [DataRow("CancellationToken.None", false)]
    [DataRow("unrelatedToken", false)]
    public async Task Checks_Captured_Helper_Token_In_TaskRun_Lambda(string argument, bool forwardsToken)
    {
        var call = $"CreateTemplateXml({argument})";
        if (!forwardsToken)
        {
            call = $"{{|#0:{call}|}}";
        }

        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                CreateTemplateZip(CancellationToken.None, cancellationToken);

            private static Task<string> CreateTemplateZip(CancellationToken unrelatedToken, CancellationToken token) =>
                Task.Run(async () =>
                {
                    return await {{call}};
                }, token);

            private static Task<string> CreateTemplateXml(CancellationToken token = default) => Task.FromResult("xml");
            """);

        await VerifyCallAsync(source, forwardsToken, "CreateTemplateXml");
    }

    [TestMethod]
    [DataRow("cancellationToken", true)]
    [DataRow("", false)]
    [DataRow("CancellationToken.None", false)]
    [DataRow("new CancellationToken()", false)]
    public async Task Checks_Module_Token_In_TaskRun_Lambda(string argument, bool forwardsToken)
    {
        var call = string.IsNullOrEmpty(argument) ? "Task.Delay(1000)" : $"Task.Delay(1000, {argument})";
        if (!forwardsToken)
        {
            call = $"{{|#0:{call}|}}";
        }

        var source = ModuleSource($$"""
            protected override async Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                await Task.Run(async () =>
                {
                    await {{call}};
                }, cancellationToken);
            }
            """);

        await VerifyCallAsync(source, forwardsToken, "Delay");
    }

    [TestMethod]
    [DataRow("async () => await Task.Delay(1, token)")]
    [DataRow("() => Task.Delay(1, token)")]
    [DataRow("async delegate { await Task.Delay(1, token); }")]
    [DataRow("Work")]
    public async Task Recognizes_Helper_Token_Captured_By_Nested_Callables(string callback)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static async Task HelperAsync(CancellationToken token)
            {
                await Task.Run({{callback}}, token);

                Task Work() => Task.Delay(1, token);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute", false)]
    [DataRow("execute", true)]
    [DataRow("lifecycle", false)]
    [DataRow("lifecycle", true)]
    [DataRow("configuration", false)]
    [DataRow("configuration", true)]
    public async Task Checks_Helper_Token_In_Nested_Lambda_For_Each_Cancellation_Root(string root, bool forwardsToken)
    {
        var entry = root switch
        {
            "execute" => "protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);",
            "lifecycle" => "protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);",
            _ => "protected override void Configure(ModuleConfigurationBuilder module) => module.WithSkipWhen(async (context, cancellationToken) => { await HelperAsync(cancellationToken); return false; });",
        };
        var call = forwardsToken ? "Task.Delay(1, token)" : "{|#0:Task.Delay(1)|}";
        var source = ModuleSource($$"""
            {{(root == "execute" ? "" : "protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.CompletedTask;")}}
            {{entry}}

            private static Task HelperAsync(CancellationToken token)
            {
                return RunAsync();

                Task RunAsync() => Task.Run(async () =>
                {
                    await {{call}};
                }, token);
            }
            """);

        await VerifyCallAsync(source, forwardsToken, "Delay");
    }

    [TestMethod]
    public async Task Reports_Helper_Token_Not_Flowed_On_Every_Call_Path()
    {
        var source = ModuleSource("""
            protected override async Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                await HelperAsync(cancellationToken, CancellationToken.None);
                await HelperAsync(CancellationToken.None, cancellationToken);
            }

            private static Task HelperAsync(CancellationToken firstToken, CancellationToken secondToken) =>
                {|#1:Task.Run(async () =>
                {
                    await {|#0:Task.Delay(1, firstToken)|};
                }, firstToken)|};
            """);

        // Neither helper parameter is guaranteed to carry the module token.
        // Task.Run and its awaited callback must both be diagnosed.
        await VerifyCS.VerifyAnalyzerAsync(
            source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(1).WithArguments("Run"));
    }

    [TestMethod]
    [DataRow("token = CancellationToken.None;", false)]
    [DataRow("token = default;", false)]
    [DataRow("Reset(ref token);", false)]
    [DataRow("ResetOut(out token);", false)]
    [DataRow("(token, _) = (CancellationToken.None, 0);", false)]
    [DataRow("token = CancellationToken.None;", true)]
    [DataRow("void ResetLocal() { token = default; } ResetLocal();", false)]
    [DataRow("void ResetLocal() { token = default; } Action reset = ResetLocal; reset();", false)]
    [DataRow("Action reset = () => token = default; Store(reset);", false)]
    [DataRow("void ResetLocal<T>() { token = default; } Store(ResetLocal<int>);", false)]
    [DataRow("var other = default(CancellationToken); (Environment.TickCount > 0 ? ref token : ref other) = default;", false)]
    [DataRow("ref var alias = ref token; alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; alias = (alias = ref other);", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; (alias, _) = ((alias = ref other), 0);", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; alias = ref (alias = ref token); alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; alias = ref token; alias = default;", false)]
    [DataRow("ref var first = ref token; ref var second = ref first; second = default;", false)]
    [DataRow("ref var alias = ref token; Reset(ref alias);", false)]
    [DataRow("ref var alias = ref token; ResetOut(out alias);", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; alias = ref (Environment.TickCount > 0 ? ref token : ref other); alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { alias = default; alias = ref token; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { alias = ref token; if (Environment.TickCount > 0) break; alias = ref other; } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; for (; Environment.TickCount > 0; alias = ref other) { alias = ref token; if (Environment.TickCount > 0) break; } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; do { alias = ref token; if (Environment.TickCount > 0) break; alias = ref other; } while (Environment.TickCount > 0); alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { alias = default; alias = ref token; if (Environment.TickCount > 0) continue; alias = ref other; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { try { alias = ref other; break; } finally { alias = ref token; } } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { try { alias = ref token; break; } finally { alias = default; } }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { while (Environment.TickCount > 0) { alias = ref token; break; } if (Environment.TickCount > 0) break; alias = ref other; } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; for (alias = ref other; Environment.TickCount > 0;) { alias = default; alias = ref token; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; if (Environment.TickCount > 0) { alias = ref other; } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; if (Environment.TickCount > 0) { alias = ref other; } else { alias = default; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; try { alias = ref other; } finally { token = default; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; switch (Environment.TickCount) { case 0: alias = ref other; break; } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; _ = Environment.TickCount > 0 && (alias = ref other).CanBeCanceled; alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; try { Read(token); alias = ref other; } finally { alias = default; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; again: alias = default; alias = ref token; if (Environment.TickCount > 0) goto again;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; _ = Environment.GetEnvironmentVariable(\"X\") ?? (alias = ref other).ToString(); alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; _ = Environment.GetEnvironmentVariable(\"X\")?.Contains((alias = ref other).ToString()!); alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; var value = Environment.GetEnvironmentVariable(\"X\"); value ??= (alias = ref other).ToString(); alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; switch (Environment.TickCount) { case int n when (alias = ref token).CanBeCanceled && n > 0: break; default: alias = default; break; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; switch (Environment.TickCount) { default: alias = default; break; case int n when (alias = ref token).CanBeCanceled && n > 0: break; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; _ = Environment.TickCount switch { int n when (alias = ref token).CanBeCanceled && n > 0 => 0, _ => (alias = default).GetHashCode() };", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { throw new Exception(); } catch (Exception) when ((alias = ref token).CanBeCanceled && Environment.TickCount > 0) { } catch (Exception) { alias = default; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { Read(token); } catch (InvalidOperationException) when ((alias = ref token).CanBeCanceled && Environment.TickCount > 0) { } catch (Exception) { alias = default; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { throw new Exception(); } catch (Exception) when ((alias = ref token).CanBeCanceled && Environment.TickCount > 0) { } catch (Exception) { } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; try { Read(token); } catch (InvalidOperationException) when ((alias = ref other).CanBeCanceled) { } catch (Exception) { alias = default; }", false)]
    public async Task Reports_Overwritten_Helper_Token_Captured_By_Callback(string write, bool insideCallback)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                {{(insideCallback ? "" : write)}}
                return Task.Run(async () =>
                {
                    {{(insideCallback ? write : "")}}
                    await {|#0:Task.Delay(1, token)|};
                }, token);
            }

            private static void Read(CancellationToken token) { }
            private static void Reset(ref CancellationToken token) => token = default;
            private static void ResetOut(out CancellationToken token) => token = default;
            private static void Store(Action callback) { }
            """);

        await VerifyCallAsync(source, false, "Delay");
    }

    [TestMethod]
    [DataRow("reset.Reset(ref token);", false)]
    [DataRow("reset.Reset(out token);", false)]
    [DataRow("ref var alias = ref token; reset.Reset(ref alias);", false)]
    [DataRow("ref var alias = ref token; reset.Reset(out alias);", false)]
    [DataRow("reset.Read(token);", true)]
    [DataRow("var other = default(CancellationToken); reset.Reset(ref other);", true)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; alias = ref other; reset.Reset(ref alias);", true)]
    public async Task Tracks_Dynamic_Argument_Writes_To_Captured_Helper_Token(string statement, bool flows)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                dynamic reset = new object();
                {{statement}}
                return Task.Run(async () => await {{(flows ? "Task.Delay(1, token)" : "{|#0:Task.Delay(1, token)|}")}}, token);
            }
            """);

        await VerifyCallAsync(source, flows, "Delay");
    }

    [TestMethod]
    [DataRow("_ = new Resetter((dynamic)0, ref token);", false)]
    [DataRow("_ = new OutResetter((dynamic)0, out token);", false)]
    [DataRow("ref var alias = ref token; _ = new Resetter((dynamic)0, ref alias);", false)]
    [DataRow("ref var alias = ref token; _ = new OutResetter((dynamic)0, out alias);", false)]
    [DataRow("_ = new Reader((dynamic)0, token);", true)]
    [DataRow("var other = default(CancellationToken); _ = new Resetter((dynamic)0, ref other);", true)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; alias = ref other; _ = new Resetter((dynamic)0, ref alias);", true)]
    public async Task Tracks_Dynamic_Constructor_Writes_To_Captured_Helper_Token(string statement, bool flows)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                {{statement}}
                return Task.Run(async () => await {{(flows ? "Task.Delay(1, token)" : "{|#0:Task.Delay(1, token)|}")}}, token);
            }

            private sealed class Resetter
            {
                public Resetter(int value, ref CancellationToken token) => token = default;
            }

            private sealed class OutResetter
            {
                public OutResetter(int value, out CancellationToken token) => token = default;
            }

            private sealed class Reader
            {
                public Reader(int value, CancellationToken token) { }
            }
            """);

        await VerifyCallAsync(source, flows, "Delay");
    }

    [TestMethod]
    [DataRow("Read(in token);")]
    [DataRow("if (false) { token = default; }")]
    [DataRow("void ResetLocal() { token = default; }")]
    [DataRow("var other = default(CancellationToken); (false ? ref token : ref other) = default;")]
    [DataRow("ref readonly var alias = ref token; _ = alias.CanBeCanceled;")]
    [DataRow("ref var alias = ref token; _ = alias.CanBeCanceled;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; alias = (alias = ref token);")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; (alias, _) = ((alias = ref token), 0);")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; alias = ref (alias = ref other); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; switch (Environment.TickCount) { case int n when (alias = ref token).CanBeCanceled && n > 0: break; case var _ when (alias = ref other).CanBeCanceled: break; default: alias = default; break; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; switch (Environment.TickCount) { case int n when (alias = ref other).CanBeCanceled && n > 0: break; default: alias = default; break; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; _ = Environment.TickCount switch { int n when (alias = ref token).CanBeCanceled && n > 0 => 0, var _ when (alias = ref other).CanBeCanceled => 1, _ => (alias = default).GetHashCode() };")]
    [DataRow("ref var alias = ref token; Read(in alias);")]
    [DataRow("ref var alias = ref token; if (false) { alias = default; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; alias = ref token; _ = alias.CanBeCanceled;")]
    [DataRow("ref var first = ref token; ref var second = ref first; _ = second.CanBeCanceled;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref (false ? ref token : ref other); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; alias = default; alias = ref token;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; ReadWrite(ref alias); alias = ref token;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; ref var second = ref alias; second = default; alias = ref token;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; alias = ref other; alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { alias = ref token; alias = ref other; if (Environment.TickCount > 0) break; } alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { alias = ref token; alias = ref other; if (Environment.TickCount > 0) continue; alias = default; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { switch (Environment.TickCount) { case 0: alias = ref token; break; } alias = ref other; } alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; for (; Environment.TickCount > 0; alias = ref other) { alias = default; alias = ref token; continue; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { try { alias = ref token; break; } finally { alias = ref other; } } alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; alias = ref other; ref var second = ref alias; second = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; if (Environment.TickCount > 0) { alias = ref other; } else { alias = ref other; } alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; if (true) { alias = ref other; } alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { alias = ref other; alias = default; alias = ref token; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; switch (Environment.TickCount) { case 0: alias = ref other; break; default: alias = ref other; break; } alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { alias = default; alias = ref token; } finally { Read(in token); }")]
    [DataRow("var dictionary = new System.Collections.Generic.Dictionary<CancellationToken, int>(); dictionary[token] = 1;")]
    [DataRow("var dictionary = new System.Collections.Generic.Dictionary<CancellationToken, int>(); (dictionary[token], _) = (1, 2);")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; _ = Environment.GetEnvironmentVariable(\"X\") ?? (alias = ref other).ToString(); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; _ = Environment.GetEnvironmentVariable(\"X\")?.Contains((alias = ref other).ToString()!); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; switch (Environment.TickCount) { case int n when (alias = ref other).CanBeCanceled && n > 0: alias = default; break; default: alias = ref other; alias = default; break; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; _ = Environment.TickCount switch { int n when (alias = ref other).CanBeCanceled && n > 0 => (alias = default).GetHashCode(), _ => (alias = ref other).GetHashCode() }; alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; _ = ((string?)null) ?? (alias = ref other).ToString(); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; _ = \"present\" ?? (alias = ref token).ToString(); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; _ = \"present\"?.Contains((alias = ref other).ToString()!); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; _ = ((string?)null)?.Contains((alias = ref token).ToString()!); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; try { throw new Exception(); } catch (Exception) when ((alias = ref other).CanBeCanceled && Environment.TickCount > 0) { } catch (Exception) { alias = default; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { throw new Exception(); } catch (Exception) when ((alias = ref token).CanBeCanceled && Environment.TickCount > 0) { } catch (Exception) when ((alias = ref other).CanBeCanceled) { } catch (Exception) { alias = default; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { throw new Exception(); } catch (Exception) when ((alias = ref token).CanBeCanceled && Environment.TickCount > 0) { } catch (Exception) { alias = ref other; alias = default; }")]
    public async Task Recognizes_Unmodified_Helper_Token_Captured_By_Callback(string statement)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                {{statement}}
                return Task.Run(async () => await Task.Delay(1, token), token);
            }

            private static void Read(in CancellationToken token) { }
            private static void ReadWrite(ref CancellationToken token) { }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("token", "other", true, false)]
    [DataRow("other", "token", false, false)]
    [DataRow("token", "token", true, false)]
    [DataRow("other", "other", false, false)]
    [DataRow("token", "other", true, true)]
    [DataRow("other", "token", false, true)]
    public async Task Tracks_Deconstruction_Destinations_In_Evaluation_Order(string initial, string rebound, bool reports, bool nested)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref {{initial}};
                var slots = new CancellationToken[1];
                {{(nested ? "((alias, _), " : "(alias, ")}}slots[(alias = ref {{rebound}}).CanBeCanceled ? 0 : 0]) =
                    {{(nested ? "((CancellationToken.None, 0), CancellationToken.None)" : "(CancellationToken.None, CancellationToken.None)")}};
                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("tokens[0] == (alias = ref other)", false, true)]
    [DataRow("MayThrow() | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("(alias = ref token).CanBeCanceled | tokens[0].CanBeCanceled | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("(1 / tokens.Length > 0) | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("tokens[0] == (alias = ref other)", true, false)]
    [DataRow("(alias = ref other).CanBeCanceled | MayThrow()", false, false)]
    [DataRow("(alias = ref other).CanBeCanceled | tokens[0].CanBeCanceled", false, false)]
    [DataRow("tokens[(alias = ref other).GetHashCode()] == other", false, false)]
    [DataRow("choice switch { 1 => (alias = ref other).CanBeCanceled }", false, true)]
    [DataRow("choice switch { _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("(dynamicNumber += 1) is not null | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("(dynamicNumber += 1) is not null | (alias = ref other).CanBeCanceled", true, false)]
    [DataRow("(alias = ref other).CanBeCanceled | (dynamicNumber += 1) is not null", false, false)]
    [DataRow("(floating /= 2d) > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("(floating %= 2d) > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("(floating / 2d) > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("(floating % 2d) > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("(single /= 2f) > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("(nullableFloating %= 2d) > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("checked(floating += 2d) > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("checked(number += 1) > 0 | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("(number /= choice) > 0 | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("(number %= choice) > 0 | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("(operand += 1) is not null | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("checked(number += 1) > 0 | (alias = ref other).CanBeCanceled", true, false)]
    [DataRow("(alias = ref other).CanBeCanceled | checked(number += 1) > 0", false, false)]
    [DataRow("unchecked(number += 1) > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("checked(++number) > 0 | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("(alias = ref other).CanBeCanceled | checked(++number) > 0", false, false)]
    [DataRow("(long)number > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("unchecked(++number) > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("checked((long)number) > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("checked((byte)number) > 0 | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("((object?)tokens is null) | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("((CancellationToken)(object)tokens).CanBeCanceled | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("(choice > 0) switch { true => (alias = ref other).CanBeCanceled, false => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("(choice > 0) switch { true when choice > 1 => (alias = ref other).CanBeCanceled, false => (alias = ref other).CanBeCanceled }", false, true)]
    [DataRow("(new Thrower((dynamic)0) is not null) | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("(new Thrower((dynamic)0) is not null) | (alias = ref other).CanBeCanceled", true, false)]
    [DataRow("(alias = ref other).CanBeCanceled | (new Thrower((dynamic)0) is not null)", false, false)]
    [DataRow("(new Thrower((dynamic)(alias = ref other)) is not null)", false, false)]
    public async Task Tracks_Throwing_Exception_Filter_Bindings(string filter, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                var tokens = Array.Empty<CancellationToken>();
                var choice = Environment.TickCount;
                var number = int.MaxValue;
                dynamic dynamicNumber = new object();
                double floating = 1;
                float single = 1;
                double? nullableFloating = 1;
                var operand = new ThrowingOperand();
                try { throw new Exception(); }
                catch (Exception) when ({{filter}}) { }
                catch (Exception)
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private static bool MayThrow() => throw new Exception();

            private sealed class ThrowingOperand
            {
                public static ThrowingOperand operator +(ThrowingOperand left, int right) => throw new Exception();
            }

            private sealed class Thrower
            {
                public Thrower(int value) => throw new Exception();
                public Thrower(CancellationToken value) => throw new Exception();
            }

            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("new ThrowingList { (alias = ref other) }", false, true)]
    [DataRow("new ThrowingList((dynamic)0) { (alias = ref other) }", false, true)]
    [DataRow("new ThrowingList { (alias = ref other) }", true, false)]
    [DataRow("new ThrowingList((dynamic)0) { (alias = ref other) }", true, false)]
    [DataRow("new ThrowingList((alias = ref other)) { other }", false, false)]
    [DataRow("new ThrowingList((dynamic)(alias = ref other)) { other }", false, false)]
    public async Task Tracks_Constructor_Exceptions_Before_Collection_Initializers(string creation, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                try { throw new Exception(); }
                catch (Exception) when ({{creation}} is not null) { }
                catch (Exception)
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private sealed class ThrowingList : List<CancellationToken>
            {
                public ThrowingList() => throw new Exception();
                public ThrowingList(int value) => throw new Exception();
                public ThrowingList(CancellationToken value) => throw new Exception();
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("state.Value", "token", false, true)]
    [DataRow("state[0]", "token", false, true)]
    [DataRow("state.Value", "token", true, false)]
    [DataRow("state[0]", "token", true, false)]
    [DataRow("state.Value", "other", false, false)]
    [DataRow("state[0]", "other", false, false)]
    public async Task Tracks_Property_Setter_Exceptions_After_Assignment_Value(string target, string rebound, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref other;
                var state = new ThrowingState();
                try { throw new Exception(); }
                catch (Exception) when (Check({{target}} = (alias = ref {{rebound}}), (alias = ref other))) { }
                catch (Exception)
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private static bool Check(CancellationToken first, CancellationToken second) => false;

            private sealed class ThrowingState
            {
                public CancellationToken Value { set => throw new Exception(); }
                public CancellationToken this[int index] { set => throw new Exception(); }
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("dynamic number = new object();", "(number + 1) is not null | (alias = ref other).CanBeCanceled", true)]
    [DataRow("dynamic number = new object();", "(-number) is not null | (alias = ref other).CanBeCanceled", true)]
    [DataRow("dynamic number = new object();", "(++number) is not null | (alias = ref other).CanBeCanceled", true)]
    [DataRow("dynamic number = new object();", "(alias = ref other).CanBeCanceled | (number + 1) is not null", false)]
    [DataRow("dynamic number = new object();", "(alias = ref other).CanBeCanceled | (-number) is not null", false)]
    [DataRow("dynamic number = new object();", "(alias = ref other).CanBeCanceled | (++number) is not null", false)]
    [DataRow("int number = 1; dynamic increment = new object();", "(number += increment) > 0 | (alias = ref other).CanBeCanceled", true)]
    public async Task Tracks_Dynamic_Arithmetic_Exception_Filter_Bindings(string declaration, string filter, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                {{declaration}}
                try { throw new Exception(); }
                catch (Exception) when ({{filter}}) { }
                catch (Exception) { alias = default; }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("state switch { { Value: true } => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, true)]
    [DataRow("state switch { (true, _) => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, true)]
    [DataRow("state switch { [true] => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, true)]
    [DataRow("state switch { [.. [true]] => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, true)]
    [DataRow("state is (true, _) | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("state is [true] | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("state switch { { Value: true } => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", true, false)]
    [DataRow("(alias = ref other).CanBeCanceled | (state switch { { Value: true } => false, _ => false })", false, false)]
    [DataRow("state switch { not null => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("state switch { PatternState _ => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("state switch { { Field: true } => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("tokens switch { [true] => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("tokens switch { [.. [true]] => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("text switch { ['a'] => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("text switch { [.. ['a']] => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("(true, false) switch { (true, _) => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    public async Task Tracks_Exception_Filter_Pattern_Bindings(string filter, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                var state = new PatternState();
                var tokens = Array.Empty<bool>();
                var text = "text";
                try { throw new Exception(); }
                catch (Exception) when ({{filter}}) { }
                catch (Exception)
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private sealed class PatternState
            {
                public bool Value => throw new Exception();
                public bool Field;
                public int Length => throw new Exception();
                public bool this[int index] => throw new Exception();
                public PatternState Slice(int start, int length) => throw new Exception();
                public void Deconstruct(out bool first, out bool second) => throw new Exception();
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    public async Task Reports_Helper_Token_Changed_After_Callback_Is_Scheduled()
    {
        var source = ModuleSource("""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var work = Task.Run(async () => await {|#0:Task.Delay(1, token)|}, token);
                token = default;
                return work;
            }
            """);

        await VerifyCallAsync(source, false, "Delay");
    }

    private static string ModuleSource(string members) => $$"""
        {{TestSourceConstants.StandardModuleHeader}}

        public sealed class DummyModule : Module
        {
            {{members}}
        }
        """;

    private static Task VerifyCallAsync(string source, bool forwardsToken, string methodName) =>
        forwardsToken
            ? VerifyCS.VerifyAnalyzerAsync(source)
            : VerifyCS.VerifyAnalyzerAsync(
                source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0)
                    .WithArguments(methodName));
}
