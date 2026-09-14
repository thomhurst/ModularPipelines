import importlib.util
import os
from pathlib import Path
import platform
import subprocess
import sys
import tempfile
import unittest
from unittest import mock


spec = importlib.util.spec_from_file_location(
    'compiler_trace', Path(__file__).with_name('trace-public-api-compiler.py'))
compiler_trace = importlib.util.module_from_spec(spec)
spec.loader.exec_module(compiler_trace)


class CompilerSelectionTests(unittest.TestCase):
    def test_recycled_pid_is_not_selected_as_the_original_compiler(self):
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            process = root / '102'
            process.mkdir()
            (process / 'status').write_text('PPid:\t100\n')
            (process / 'stat').write_text('102 (compiler) S ' + '0 ' * 18 + '1234')
            (process / 'cmdline').write_bytes(b'/sdk/Roslyn/bincore/csc\0/out:Azure.dll')

            def replace_process(*_arguments):
                (process / 'stat').write_text('102 (replacement) S ' + '0 ' * 18 + '5678')
                return True

            with mock.patch.object(compiler_trace, 'is_descendant', side_effect=replace_process):
                self.assertEqual([], list(compiler_trace.find_compilers(100, 'Azure', root)))

    def test_only_target_project_compilers_owned_by_sync_are_selected(self):
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)

            def process(pid, parent, arguments):
                path = root / str(pid)
                path.mkdir()
                (path / 'status').write_text(f'PPid:\t{parent}\n')
                (path / 'cmdline').write_bytes('\0'.join(arguments).encode())
                (path / 'stat').write_text(f'{pid} (compiler) S ' + '0 ' * 18 + str(pid * 10))

            command = ['/sdk/Roslyn/bincore/csc', '/out:obj/Release/net10.0/Azure.dll']
            process(100, 1, ['pwsh'])
            process(101, 100, ['dotnet', 'build'])
            process(102, 101, command)
            process(103, 1, command)  # Same project in an unrelated build.
            process(104, 101, [command[0], '/out:obj/Release/net10.0/Core.dll'])
            process(105, 101, ['echo', *command])  # Merely mentions the compiler.
            process(106, 101, ['dotnet', '/sdk/Roslyn/bincore/csc.dll', command[1]])
            response_file = root / 'compiler options.rsp'
            response_file.write_text('/nologo\n/out:"obj/Release/net10.0/Azure.dll"\n', encoding='utf-8-sig')
            process(107, 101, [command[0], '@' + str(response_file)])
            process(108, 1, [command[0], '@' + str(response_file)])
            process(109, 101, [command[0], '@missing.rsp'])
            self.assertEqual([(102, 1020), (106, 1060), (107, 1070)],
                             sorted(compiler_trace.find_compilers(100, 'Azure', root)))

    def test_disappearing_process_or_parent_cycle_is_not_owned(self):
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            (root / '101').mkdir()
            (root / '101' / 'status').write_text('PPid:\t101\n')
            self.assertFalse(compiler_trace.is_descendant(101, 100, root))
            self.assertFalse(compiler_trace.is_descendant(102, 100, root))

    def test_recycled_pid_does_not_inherit_the_previous_compilers_wait(self):
        build = mock.Mock(pid=100, returncode=0)
        build.poll.side_effect = [None, None, None, 0]
        trace = mock.Mock()
        trace.poll.return_value = 0
        log = mock.Mock()
        with (
            mock.patch.object(sys, 'argv', ['collector', '--project', 'Azure', '--trace-tool', 'trace',
                                           '--output-directory', '/output', '--', 'build']),
            mock.patch.object(compiler_trace.platform, 'system', return_value='Linux'),
            mock.patch.object(compiler_trace.platform, 'platform', return_value='Linux-test'),
            mock.patch.object(compiler_trace.signal, 'signal'),
            mock.patch.object(compiler_trace, 'stop_owned_process'),
            mock.patch.object(compiler_trace, 'find_compilers',
                              side_effect=[[(101, 1000)], [(101, 2000)], [(101, 2000)]]),
            mock.patch.object(compiler_trace, 'process_start_time', return_value=2000),
            mock.patch.object(compiler_trace.time, 'monotonic', side_effect=[0, 121, 242, 242]) as clock,
            mock.patch.object(compiler_trace.time, 'sleep'),
            mock.patch.object(Path, 'read_text', return_value='status'),
            mock.patch.object(Path, 'write_text'),
            mock.patch('builtins.open', return_value=log),
            mock.patch.object(compiler_trace.subprocess, 'Popen') as popen,
        ):
            def launch(command, **_kwargs):
                if command == ['build']:
                    return build
                self.assertEqual(3, clock.call_count)
                return trace

            popen.side_effect = launch
            self.assertEqual(0, compiler_trace.main())
            self.assertEqual(2, popen.call_count)
        log.close.assert_called_once()

    def test_failed_trace_start_allows_a_later_compiler_and_closes_logs(self):
        for failure in ('proc', 'launch'):
            with self.subTest(failure=failure):
                build = mock.Mock(pid=100, returncode=7)
                build.poll.side_effect = [None, None, None, 0]
                trace = mock.Mock()
                trace.poll.return_value = 0
                logs = [mock.Mock(), mock.Mock()]
                launches = [build, trace] if failure == 'proc' else [build, OSError('trace launch failed'), trace]
                reads = [FileNotFoundError('compiler exited'), 'status', 'stat'] if failure == 'proc' else ['status'] * 4
                with (
                    mock.patch.object(sys, 'argv', ['collector', '--project', 'Azure', '--trace-tool', 'trace',
                                                   '--output-directory', '/output', '--', 'build']),
                    mock.patch.object(compiler_trace.platform, 'system', return_value='Linux'),
                    mock.patch.object(compiler_trace.platform, 'platform', return_value='Linux-test'),
                    mock.patch.object(compiler_trace.signal, 'signal'),
                    mock.patch.object(compiler_trace, 'stop_owned_process'),
                    mock.patch.object(compiler_trace, 'find_compilers',
                                      side_effect=[[(101, 1010)], [(101, 1010), (102, 1020)], [(102, 1020)]]),
                    mock.patch.object(compiler_trace, 'process_start_time', side_effect=lambda pid: pid * 10),
                    mock.patch.object(compiler_trace.time, 'monotonic', side_effect=[0, 121, 242, 242]),
                    mock.patch.object(compiler_trace.time, 'sleep'),
                    mock.patch.object(Path, 'read_text', side_effect=reads),
                    mock.patch.object(Path, 'write_text'),
                    mock.patch('builtins.open', side_effect=logs),
                    mock.patch.object(compiler_trace.subprocess, 'Popen', side_effect=launches) as popen,
                ):
                    self.assertEqual(7, compiler_trace.main())
                self.assertEqual('102', popen.call_args.args[0][3])
                logs[0].close.assert_called_once()
                if failure == 'launch':
                    logs[1].close.assert_called_once()

    @unittest.skipUnless(platform.system() == 'Linux', 'The collector uses Linux /proc.')
    def test_build_failure_exit_code_is_preserved_without_a_compiler(self):
        with tempfile.TemporaryDirectory() as directory:
            result = subprocess.run(
                [sys.executable, str(Path(__file__).with_name('trace-public-api-compiler.py')),
                 '--project', 'Azure', '--trace-tool', '/unused', '--output-directory', directory,
                 '--', sys.executable, '-c', 'raise SystemExit(7)'], timeout=10)
            self.assertEqual(7, result.returncode)

    @unittest.skipUnless(platform.system() == 'Linux', 'Process group cleanup is Linux-only.')
    def test_cleanup_terminates_the_owned_process_group(self):
        process = subprocess.Popen([sys.executable, '-c', 'import time; time.sleep(60)'],
                                   start_new_session=True)
        try:
            self.assertEqual(process.pid, os.getpgid(process.pid))
            compiler_trace.stop_owned_process(process)
            self.assertIsNotNone(process.poll())
        finally:
            if process.poll() is None:
                process.kill()
                process.wait()


if __name__ == '__main__':
    unittest.main()
