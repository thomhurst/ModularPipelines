using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleTaskRunCancellationFlowAnalyzerTests
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
    [DataRow("void ResetLocal() { token = default; } Action reset = () => ResetLocal(); Store(reset);", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { if (Environment.TickCount > 0) { alias = ref token; return Task.CompletedTask; } } finally { alias = default; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { alias = ref other; } finally { try { throw new Exception(); } catch (Exception) { alias = ref token; } } alias = default;", false)]
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

        var warnsAtRun = !insideCallback && write is "token = CancellationToken.None;" or "token = default;";
        if (warnsAtRun)
        {
            source = source.Replace("return Task.Run(", "return {|#1:Task.Run(")
                .Replace("}, token);", "}, token)|};");
        }

        await VerifyCS.VerifyAnalyzerAsync(source, warnsAtRun
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay"),
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(1).WithArguments("Run")]
            : [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]);
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
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; if (Environment.TickCount > 0) { alias = ref token; return Task.CompletedTask; } alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { if (Environment.TickCount > 0) { alias = ref token; return Task.CompletedTask; } } finally { Read(in token); } alias = default;")]
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
    [DataRow("((dynamic)state).Value", "token", false, true)]
    [DataRow("((dynamic)state)[0]", "token", false, true)]
    [DataRow("((dynamic)state).Value", "token", true, false)]
    [DataRow("((dynamic)state)[0]", "token", true, false)]
    [DataRow("((dynamic)state).Value", "other", false, false)]
    [DataRow("((dynamic)state)[0]", "other", false, false)]
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
    [DataRow("state.Value", "token", false, true, false)]
    [DataRow("state[0]", "token", false, true, false)]
    [DataRow("slots[0]", "token", false, true, false)]
    [DataRow("state.Value", "token", true, false, false)]
    [DataRow("state[0]", "token", true, false, false)]
    [DataRow("slots[0]", "token", true, false, false)]
    [DataRow("state.Value", "other", false, false, false)]
    [DataRow("state[0]", "other", false, false, false)]
    [DataRow("slots[0]", "other", false, false, false)]
    [DataRow("((dynamic)state).Value", "token", false, true, false)]
    [DataRow("((dynamic)state)[0]", "token", false, true, false)]
    [DataRow("state.Value", "token", false, true, true)]
    [DataRow("state.Value", "token", true, false, true)]
    [DataRow("local", "token", false, false, true)]
    public async Task Tracks_Deconstruction_Store_Exceptions_After_Assignment_Value(string target, string rebound, bool rebindInHandler, bool reports, bool nested)
    {
        var assignment = nested
            ? $"(({target}, _), _) = (((alias = ref {rebound}), 0), 0)"
            : $"({target}, _) = ((alias = ref {rebound}), 0)";
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref other;
                var state = new ThrowingState();
                object[] slots = new string[1];
                var local = default(CancellationToken);
                try { throw new Exception(); }
                catch (Exception) when (Check({{assignment}}, (alias = ref other))) { }
                catch (Exception)
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private static bool Check(object first, CancellationToken second) => false;

            private sealed class ThrowingState
            {
                public CancellationToken Value { set => throw new Exception(); }
                public CancellationToken this[int index] { set => throw new Exception(); }
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("object[] slots = new string[1];", "token", false, true)]
    [DataRow("CancellationToken[] slots = new CancellationToken[0];", "token", false, true)]
    [DataRow("CancellationToken[] slots = null!;", "token", false, true)]
    [DataRow("object[] slots = new string[1];", "token", true, false)]
    [DataRow("object[] slots = new string[1];", "other", false, false)]
    public async Task Tracks_Array_Store_Exceptions_After_Assignment_Value(string declaration, string rebound, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref other;
                {{declaration}}
                try { throw new Exception(); }
                catch (Exception) when (Check(slots[0] = (alias = ref {{rebound}}), (alias = ref other))) { }
                catch (Exception)
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private static bool Check(object first, CancellationToken second) => false;
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("ThrowingEquality", "left == right", false, true)]
    [DataRow("ThrowingEquality", "left != right", false, true)]
    [DataRow("ThrowingEquality?", "left == right", false, true)]
    [DataRow("(ThrowingEquality, int)", "left == right", false, true)]
    [DataRow("ThrowingEquality", "left == right", true, false)]
    [DataRow("UnrelatedEquality", "left == right", false, false)]
    [DataRow("UnrelatedEquality", "left != right", false, false)]
    [DataRow("int", "left == right", false, false)]
    [DataRow("int", "left != right", false, false)]
    [DataRow("string?", "left == right", false, false)]
    [DataRow("decimal", "left == right", false, false)]
    [DataRow("ThrowingEquality", "left == right", false, true, "?")]
    [DataRow("ThrowingEquality", "left != right", true, false, "?")]
    [DataRow("int", "left == right", false, false, "?")]
    public async Task Tracks_Tuple_Equality_Exceptions(string elementType, string comparison, bool rebindInHandler, bool reports, string nullable = "")
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                ({{elementType}}, int){{nullable}} left = default, right = default;
                try { throw new Exception(); }
                catch (Exception) when (({{comparison}}) | (alias = ref other).CanBeCanceled) { }
                catch (Exception)
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private sealed class UnrelatedEquality
            {
                public static bool operator ==(UnrelatedEquality left, int right) => throw new Exception();
                public static bool operator !=(UnrelatedEquality left, int right) => throw new Exception();
                public override bool Equals(object? obj) => false;
                public override int GetHashCode() => 0;
            }

            private struct ThrowingEquality
            {
                public static bool operator ==(ThrowingEquality left, ThrowingEquality right) => throw new Exception();
                public static bool operator !=(ThrowingEquality left, ThrowingEquality right) => throw new Exception();
                public override bool Equals(object? obj) => false;
                public override int GetHashCode() => 0;
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("ConvertsToInt", "int", "==", false, true)]
    [DataRow("int", "ConvertsToInt", "!=", false, true)]
    [DataRow("ConvertsToInt?", "int?", "==", false, true)]
    [DataRow("(ConvertsToInt, int)", "(int, int)", "==", false, true)]
    [DataRow("ConvertsToInt", "int", "==", true, false)]
    [DataRow("short", "int", "==", false, false)]
    public async Task Tracks_Tuple_Equality_Operand_Conversions(string leftType, string rightType, string comparison, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);
            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                ({{leftType}}, int) left = default;
                ({{rightType}}, int) right = default;
                try { _ = left {{comparison}} right; }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }
                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            private readonly struct ConvertsToInt
            {
                public static implicit operator int(ConvertsToInt value) => throw new Exception();
            }
            """);
        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("token", "other", false, true)]
    [DataRow("other", "token", false, true)]
    [DataRow("token", "other", true, false)]
    [DataRow("other", "other", false, false)]
    public async Task Tracks_Record_With_Clone_Exception_Order(string initial, string rebound, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);
            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref {{initial}};
                var state = new CloneState();
                try { _ = state with { Value = (alias = ref {{rebound}}) }; }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }
                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            private sealed record CloneState
            {
                public CloneState() { }
                private CloneState(CloneState original) => throw new Exception();
                public CancellationToken Value { get; init; }
            }
            """);
        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("slots[0] = (alias = ref other);", false)]
    [DataRow("slots[0] = choose ? (alias = ref other) : (alias = ref other);", false)]
    [DataRow("slots[^1] = (alias = ref other);", true)]
    [DataRow("GetSlots(slots)[0] = (alias = ref other);", true)]
    [DataRow("slots[GetIndex()] = (alias = ref other);", true)]
    [DataRow("slots[(alias = ref other).GetHashCode() & 0] = token;", false)]
    public async Task Preserves_Array_Store_Exception_Order(string statement, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);
            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                var slots = new CancellationToken[1];
                bool choose = false;
                try { {{statement}} }
                catch { alias = default; }
                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            private static CancellationToken[] GetSlots(CancellationToken[] slots) => throw new Exception();
            private static int GetIndex() => throw new Exception();
            """);
        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("pair", "token", false, true)]
    [DataRow("(new ThrowingInt(), 2)", "token", false, true)]
    [DataRow("(new ThrowingInt(), 2)", "token", true, false)]
    [DataRow("pair", "token", true, false)]
    [DataRow("pair", "other", false, false)]
    [DataRow("(1, 2)", "token", false, false)]
    public async Task Tracks_Deconstruction_Method_Exceptions(string value, string initialAlias, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref {{initialAlias}};
                var pair = new ThrowingPair();
                int x, y;
                try { throw new Exception(); }
                catch (Exception) when (Check((x, y) = {{value}}, (alias = ref other))) { }
                catch (Exception)
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private static bool Check((int, int) first, CancellationToken second) => false;

            private struct ThrowingInt
            {
                public static implicit operator int(ThrowingInt value) => throw new Exception();
            }

            private sealed class ThrowingPair
            {
                public void Deconstruct(out int x, out int y) => throw new Exception();
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
    [DataRow("ThrowingInt", "int", false, true)]
    [DataRow("object", "string", false, true)]
    [DataRow("object", "int", false, true)]
    [DataRow("ThrowingInt", "int", true, false)]
    [DataRow("object", "string", true, false)]
    [DataRow("object", "int", true, false)]
    [DataRow("int", "long", false, false)]
    [DataRow("ThrowingInt", "int", false, true, "?")]
    [DataRow("ThrowingInt", "int", true, false, "?")]
    [DataRow("int", "long", false, false, "?")]
    [DataRow("int?", "int", false, true)]
    [DataRow("int?", "int", true, false)]
    [DataRow("int?", "long?", false, false)]
    [DataRow("int?", "int?", false, false)]
    [DataRow("long", "int", false, false)]
    [DataRow("decimal", "int", false, true)]
    [DataRow("decimal", "int", true, false)]
    [DataRow("double", "decimal", false, true)]
    [DataRow("float", "decimal", false, true)]
    [DataRow("decimal?", "int?", false, true)]
    [DataRow("(decimal, int)", "(int, int)", false, true)]
    [DataRow("decimal", "double", false, false)]
    [DataRow("int", "decimal", false, false)]
    [DataRow("dynamic", "int", false, true)]
    [DataRow("dynamic", "int", true, false)]
    [DataRow("dynamic", "object", false, false)]
    [DataRow("dynamic", "dynamic", false, false)]
    [DataRow("(int?, int)", "(int, int)", false, true)]
    [DataRow("(int?, int)", "(int, int)", true, false)]
    [DataRow("long", "int", false, true, "", true)]
    [DataRow("long", "int", true, false, "", true)]
    [DataRow("int", "long", false, false, "", true)]
    [DataRow("long?", "int?", false, true, "", true)]
    [DataRow("long?", "int?", true, false, "", true)]
    [DataRow("byte", "char", false, false, "", true)]
    [DataRow("ushort", "char", false, false, "", true)]
    public async Task Tracks_Hidden_Tuple_Element_Conversions(string sourceType, string targetType, bool rebindInHandler, bool reports, string nullable = "", bool checkedConversion = false)
    {
        var conversion = $"(({targetType}, int){nullable})pair";
        if (checkedConversion)
        {
            conversion = $"checked({conversion})";
        }

        var source = ModuleSource($$"""
            #pragma warning disable CS8619 // These cases intentionally unwrap nullable tuple elements.
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                ({{sourceType}}, int){{nullable}} pair = default;
                try { throw new Exception(); }
                catch (Exception) when (({{conversion}}){{(nullable.Length == 0 ? ".Item2 > 0" : " is not null")}} | (alias = ref other).CanBeCanceled) { }
                catch (Exception)
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private struct ThrowingInt
            {
                public static explicit operator int(ThrowingInt value) => throw new Exception();
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("decimal", "int", false, true)]
    [DataRow("decimal", "int", true, false)]
    [DataRow("double", "decimal", false, true)]
    [DataRow("float", "decimal", false, true)]
    [DataRow("decimal?", "int?", false, true)]
    [DataRow("decimal", "double", false, false)]
    [DataRow("int", "decimal", false, false)]
    [DataRow("long", "int", false, false)]
    public async Task Tracks_Unchecked_Scalar_Numeric_Conversion_Exceptions(string sourceType, string targetType, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken, default);

            private static Task HelperAsync(CancellationToken token, {{sourceType}} value)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                try { _ = unchecked(({{targetType}})value); }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("Wide", "int", true, false, true)]
    [DataRow("Wide", "int", true, true, false)]
    [DataRow("Wide?", "int?", true, false, true)]
    [DataRow("Wide", "Narrow", true, false, true)]
    [DataRow("long", "Narrow", true, false, true)]
    [DataRow("Wide", "int", false, false, false)]
    [DataRow("Narrow", "long", true, false, false)]
    [DataRow("Narrow", "char", true, false, false)]
    [DataRow("Wide", "Wide", true, false, false)]
    public async Task Tracks_Checked_Scalar_Enum_Conversions(string sourceType, string targetType, bool checkedConversion, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken, default);

            private static Task HelperAsync(CancellationToken token, {{sourceType}} value)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                try { _ = {{(checkedConversion ? "checked" : "unchecked")}}(({{targetType}})value); }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private enum Wide : long { Zero }
            private enum Narrow : byte { Zero }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("decimal", "value++", false, true)]
    [DataRow("decimal", "--value", false, true)]
    [DataRow("decimal", "value++", true, false)]
    [DataRow("int", "value++", false, false)]
    [DataRow("double", "--value", false, false)]
    public async Task Tracks_Unchecked_Decimal_Increment_Exceptions(string type, string expression, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken, default);

            private static Task HelperAsync(CancellationToken token, {{type}} value)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                try { unchecked { {{expression}}; } }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("await pending;", true)]
    [DataRow("throw error;", true)]
    [DataRow("await pending;", false)]
    [DataRow("throw error;", false)]
    public async Task Tracks_Await_And_Base_Typed_Throw_Exceptions(string statement, bool writes)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken, Task.CompletedTask, new InvalidOperationException());

            private static async Task HelperAsync(CancellationToken token, Task pending, Exception error)
            {
                try { {{statement}} }
                catch (InvalidOperationException) { {{(writes ? "token = default;" : "")}} }
                catch (Exception) { }
                await Task.Run(async () => await {{(writes ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            """);

        await VerifyCallAsync(source, !writes, "Delay");
    }

    [TestMethod]
    [DataRow("token", "!(alias = default).CanBeCanceled", "", true)]
    [DataRow("other", "!(alias = default).CanBeCanceled", "", false)]
    [DataRow("token", "false", "alias = default;", false)]
    [DataRow("token", "true", "alias = default;", false)]
    public async Task Searches_Filters_Before_Unwinding_Finally(string initial, string filter, string handler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);
            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref {{initial}};
                try
                {
                    try { Read(); }
                    finally { alias = ref other; }
                }
                catch (Exception) when ({{filter}}) { {{handler}} }
                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            private static void Read() { }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("ThrowingCollection", "[1]", false, true)]
    [DataRow("AddingCollection", "[1]", false, true)]
    [DataRow("BuiltCollection", "[1]", false, true)]
    [DataRow("int[]", "[.. values]", false, true)]
    [DataRow("ThrowingCollection", "[1]", true, false)]
    [DataRow("AddingCollection", "[1]", true, false)]
    [DataRow("BuiltCollection", "[1]", true, false)]
    [DataRow("int[]", "[.. values]", true, false)]
    [DataRow("int[]", "[1]", false, false)]
    [DataRow("ThrowingCollection", "[(alias = ref other).CanBeCanceled ? 1 : 2]", false, true)]
    [DataRow("BuiltCollection", "[(alias = ref other).CanBeCanceled ? 1 : 2]", false, false)]
    [DataRow("int[]", "[.. values, (alias = ref other).CanBeCanceled ? 1 : 2]", false, true)]
    [DataRow("int[]", "[(alias = ref other).CanBeCanceled ? 1 : 2, .. values]", false, false)]
    public async Task Tracks_Implicit_Collection_Exceptions(string collectionType, string expression, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken, Array.Empty<int>());

            private static Task HelperAsync(CancellationToken token, System.Collections.Generic.IEnumerable<int> values)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                try { {{collectionType}} collection = {{expression}}; }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private sealed class ThrowingCollection : System.Collections.Generic.List<int>
            {
                public ThrowingCollection() => throw new Exception();
            }

            private sealed class AddingCollection : System.Collections.Generic.List<int>
            {
                public new void Add(int value) => throw new Exception();
            }

            [System.Runtime.CompilerServices.CollectionBuilder(typeof(CollectionFactory), nameof(CollectionFactory.Create))]
            private sealed class BuiltCollection : System.Collections.Generic.IEnumerable<int>
            {
                public System.Collections.Generic.IEnumerator<int> GetEnumerator() => throw new Exception();
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            }

            private static class CollectionFactory
            {
                public static BuiltCollection Create(ReadOnlySpan<int> values) => throw new Exception();
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("_ = receiver.Ready", false, true)]
    [DataRow("_ = receiver.Ready", true, false)]
    [DataRow("receiver.Ready = null", false, true)]
    [DataRow("receiver.Ready = null", true, false)]
    [DataRow("receiver.Ready = (alias = ref other).CanBeCanceled ? null : null", false, false)]
    public async Task Tracks_Field_Like_Event_Exceptions(string statement, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            public event Action? Ready;

            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken, null!);

            private static Task HelperAsync(CancellationToken token, DummyModule receiver)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                try { {{statement}}; }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("(receiver.Ready, _) = (MakeHandler((alias = ref other)), 0)", false)]
    [DataRow("((receiver.Ready, _), _) = ((MakeHandler((alias = ref other)), 0), 0)", false)]
    [DataRow("(receiver.Ready, _) = (MakeHandler(token), 0)", true)]
    [DataRow("(receiver.Ready, _) = (MakeHandler(token), (alias = ref other).CanBeCanceled ? 0 : 1)", true)]
    [DataRow("(receiver.Ready, _) = ((Action?)null, 0)", true)]
    [DataRow("(receiver.Ready, _) = ((alias = ref other).CanBeCanceled ? null : (Action?)null, 0)", false)]
    [DataRow("(receiver.Ready, _) = (token.CanBeCanceled ? null : (Action?)null, 0)", true)]
    [DataRow("(Ready, _) = ((Action?)null, 0)", false)]
    [DataRow("(receiver.Field, _) = (MakeHandler((alias = ref other)), 0)", false)]
    [DataRow("(receiver.Field, _) = (MakeHandler(token), 0)", true)]
    [DataRow("(receiver.Field, _) = ((Action?)null, 0)", true)]
    [DataRow("(Field, _) = ((Action?)null, 0)", false)]
    public async Task Tracks_Deconstruction_Event_And_Field_Stores_After_Value(string statement, bool reports)
    {
        var source = ModuleSource($$"""
            public event Action? Ready;
            public Action? Field;

            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                DummyModule receiver = null!;
                ref var alias = ref token;
                try { {{statement}}; }
                catch { alias = default; }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private static Action MakeHandler(CancellationToken token) => () => { };
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("publisher.Changed += handler", false, true)]
    [DataRow("publisher.Changed -= handler", false, true)]
    [DataRow("publisher.Changed += handler", true, false)]
    [DataRow("publisher.Changed -= handler", true, false)]
    [DataRow("_ = Holder.Value", false, true)]
    [DataRow("_ = Holder.Value", true, false)]
    [DataRow("_ = Holder.Constant", false, false)]
    [DataRow("Publisher.StaticChanged += handler", false, true)]
    [DataRow("Publisher.StaticChanged -= handler", false, true)]
    [DataRow("Publisher.StaticChanged += handler", true, false)]
    [DataRow("Publisher.StaticChanged -= handler", true, false)]
    [DataRow("Holder.Value = 1", false, true)]
    [DataRow("Holder.Value |= 1", false, true)]
    [DataRow("Holder.Value = 1", true, false)]
    [DataRow("Holder.Value |= 1", true, false)]
    [DataRow("Holder.Value = (alias = ref other).CanBeCanceled ? 1 : 2", false, false)]
    [DataRow("Holder.Value |= (alias = ref other).CanBeCanceled ? 1 : 2", false, true)]
    public async Task Tracks_Event_Accessor_And_Static_Field_Exceptions(string statement, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken, new Publisher(), () => { });

            private static Task HelperAsync(CancellationToken token, Publisher publisher, Action handler)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                try { {{statement}}; }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private sealed class Publisher
            {
                public event Action Changed
                {
                    add => throw new Exception();
                    remove => throw new Exception();
                }

                public static event Action StaticChanged
                {
                    add => throw new Exception();
                    remove => throw new Exception();
                }
            }

            private static class Holder
            {
                static Holder() => throw new Exception();
                public static int Value = 1;
                public const int Constant = 1;
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("_ = \"prefix\" + value", false, true)]
    [DataRow("text += value", false, true)]
    [DataRow("_ = \"prefix\" + value", true, false)]
    [DataRow("text += value", true, false)]
    [DataRow("_ = text + \"suffix\"", false, false)]
    [DataRow("text += \"suffix\"", false, false)]
    public async Task Tracks_Implicit_String_Concatenation_Exceptions(string statement, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken, new ThrowingValue(), "prefix");

            private static Task HelperAsync(CancellationToken token, object value, string text)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                try { {{statement}}; }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private sealed class ThrowingValue
            {
                public override string ToString() => throw new Exception();
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("Action action = receiver.Work", false, true)]
    [DataRow("Action action = receiver.Work", true, false)]
    [DataRow("Action action = Receiver.StaticWork", false, false)]
    public async Task Tracks_Method_Group_Creation_Exceptions(string statement, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken, null!);

            private static Task HelperAsync(CancellationToken token, Receiver receiver)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                try { {{statement}}; }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private sealed class Receiver
            {
                public void Work() { }
                public static void StaticWork() { }
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("Index index = ^offset", false, true)]
    [DataRow("Index index = ^offset", true, false)]
    [DataRow("Index index = ^((alias = ref other).CanBeCanceled ? offset : offset)", false, false)]
    [DataRow("int index = -offset", false, false)]
    public async Task Tracks_From_End_Index_Construction_Exceptions(string statement, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken, -1);

            private static Task HelperAsync(CancellationToken token, int offset)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                try { {{statement}}; }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow(false, true)]
    [DataRow(true, false)]
    public async Task Tracks_Generic_Object_Construction_Exceptions(bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync<ThrowingConstructor>(cancellationToken);

            private static Task HelperAsync<T>(CancellationToken token) where T : new()
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                try { _ = new T(); }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private sealed class ThrowingConstructor
            {
                public ThrowingConstructor() => throw new InvalidOperationException();
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("action()", false, true)]
    [DataRow("action()", true, false)]
    [DataRow("withArgument((alias = ref other))", false, false)]
    public async Task Tracks_Function_Pointer_Invocation_Exceptions(string invocation, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                unsafe
                {
                    delegate* managed<void> action = &Throw;
                    delegate* managed<CancellationToken, void> withArgument = &ThrowWithArgument;
                    var other = default(CancellationToken);
                    ref var alias = ref token;
                    try { {{invocation}}; }
                    catch
                    {
                        {{(rebindInHandler ? "alias = ref other;" : "")}}
                        alias = default;
                    }
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private static void Throw() => throw new InvalidOperationException();
            private static void ThrowWithArgument(CancellationToken value) => throw new InvalidOperationException();
            """);

        var test = new VerifyCS.Test
        {
            TestCode = source,
            ReferenceAssemblies = Net.Net110,
            TestState =
            {
                AdditionalReferences = { typeof(IModuleContext).Assembly.Location },
            },
        };
        test.SolutionTransforms.Add((solution, projectId) =>
            solution.WithProjectCompilationOptions(projectId,
                ((Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions) solution.GetProject(projectId)!.CompilationOptions!).WithAllowUnsafe(true)));
        if (reports)
        {
            test.ExpectedDiagnostics.Add(VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
        }

        await test.RunAsync(CancellationToken.None);
    }

    [TestMethod]
    [DataRow("T", "", false, true)]
    [DataRow("T", "", true, false)]
    [DataRow("T", "where T : struct", false, true)]
    [DataRow("T?", "where T : struct", false, true)]
    [DataRow("T?", "where T : struct", true, false)]
    [DataRow("T?", "", false, true)]
    [DataRow("object", "", false, false)]
    public async Task Tracks_Generic_Unboxing_Exceptions(string target, string constraint, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync<int>(cancellationToken, "text");

            private static Task HelperAsync<T>(CancellationToken token, object value) {{constraint}}
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                try { _ = ({{target}})value; }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("token", true)]
    [DataRow("other", false)]
    public async Task Tracks_Aliases_Transferred_Into_Ref_Fields(string initial, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);
            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref {{initial}};
                scoped TokenSlot slot = default;
                slot.Value = ref alias;
                slot.Value = default;
                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            private ref struct TokenSlot { public ref CancellationToken Value; }
            """);
        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("other", "token", false)]
    [DataRow("token", "other", true)]
    public async Task Preserves_Ref_Return_Indexer_Exception_Order(string initial, string rebound, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);
            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref {{initial}};
                var state = new RefState();
                Index index = ^1;
                try { state[index] = (alias = ref {{rebound}}); }
                catch { alias = default; }
                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }
            private sealed class RefState
            {
                private CancellationToken _value;
                public int Length => 1;
                public ref CancellationToken this[int index] => ref _value;
            }
            """);
        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("other", "token", false, true)]
    [DataRow("other", "token", true, false)]
    [DataRow("token", "other", false, true)]
    [DataRow("other", "other", false, false)]
    [DataRow("other", "token", false, true, false)]
    [DataRow("token", "other", false, true, false)]
    public async Task Tracks_Implicit_Indexer_Store_Exceptions_After_Value(string initial, string rebound, bool rebindInHandler, bool reports, bool throwingLength = true)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref {{initial}};
                var state = new SetterState();
                Index index = ^1;
                try { state[index] = (alias = ref {{rebound}}); }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private sealed class SetterState
            {
                public int Length => {{(throwingLength ? "throw new Exception()" : "1")}};
                public CancellationToken this[int index] { set => throw new Exception(); }
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("_ = receiver[index];", false, true)]
    [DataRow("_ = receiver[index];", true, false)]
    [DataRow("int result = choice switch { _ => value };", false, true)]
    [DataRow("int result = choice switch { _ => value };", true, false)]
    [DataRow("int result = choice switch { _ => 1 };", false, false)]
    [DataRow("state.Value = (alias = ref other);", false, false)]
    [DataRow("state.Value = choice > 0 ? (alias = ref other) : (alias = ref other);", false, false)]
    [DataRow("state.Value = token;", false, true)]
    [DataRow("GetState(state).Value = (alias = ref other);", false, true)]
    [DataRow("state[(alias = ref other).GetHashCode()] = token;", false, false)]
    [DataRow("state[GetIndex()] = (alias = ref other);", false, true)]
    [DataRow("state.RefValue = (alias = ref other);", false, true)]
    public async Task Tracks_Hidden_Exceptions_And_Setter_Evaluation_Order(string statement, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                var receiver = new IndexReceiver();
                Index index = ^1;
                var state = new SetterState();
                var value = new SwitchValue();
                int choice = 0;
                try { {{statement}} }
                catch
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private sealed class IndexReceiver
            {
                public int Length => throw new Exception();
                public int this[int index] => throw new Exception();
            }

            private sealed class SetterState
            {
                private CancellationToken _value;
                public CancellationToken Value { set => throw new Exception(); }
                public CancellationToken this[int index] { set => throw new Exception(); }
                public ref CancellationToken RefValue => ref _value;
            }

            private readonly struct SwitchValue
            {
                public static implicit operator int(SwitchValue value) => throw new Exception();
            }

            private static SetterState GetState(SetterState state) => throw new Exception();
            private static int GetIndex() => throw new Exception();
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("token", "other", true, false)]
    [DataRow("other", "token", false, false)]
    [DataRow("token", "token", true, false)]
    [DataRow("other", "other", false, false)]
    [DataRow("token", "other", true, true)]
    [DataRow("other", "token", false, true)]
    public async Task Tracks_Straight_Line_Deconstruction_Destinations(string initial, string rebound, bool reports, bool nested)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref {{initial}};
                var slots = new CancellationToken[1];
                {{(nested ? "((alias, _), " : "(alias, ")}}slots[(alias = ref {{rebound}}).GetHashCode() & 0]) =
                    {{(nested ? "((CancellationToken.None, 0), CancellationToken.None)" : "(CancellationToken.None, CancellationToken.None)")}};
                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
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
