"""Run API synchronization on Linux and sample a slow, owned C# compiler once."""

import argparse
from datetime import datetime, timezone
import json
import os
from pathlib import Path
import platform
import re
import signal
import subprocess
import time


def is_descendant(process_id, ancestor_id, proc_root=Path('/proc')):
    seen = set()
    while process_id > 1 and process_id not in seen:
        if process_id == ancestor_id:
            return True
        seen.add(process_id)
        try:
            status = (proc_root / str(process_id) / 'status').read_text()
            process_id = next(int(line.split()[1]) for line in status.splitlines()
                              if line.startswith('PPid:'))
        except (OSError, StopIteration, ValueError):
            return False
    return False


def find_compilers(ancestor_id, project, proc_root=Path('/proc')):
    for directory in proc_root.iterdir():
        if not directory.name.isdigit():
            continue
        try:
            arguments = (directory / 'cmdline').read_bytes().decode().split('\0')
        except (OSError, UnicodeError):
            continue
        compiler = (arguments[0].endswith('/Roslyn/bincore/csc')
                    or (Path(arguments[0]).name == 'dotnet' and len(arguments) > 1
                        and arguments[1].endswith('/Roslyn/bincore/csc.dll')))
        process_id = int(directory.name)
        if not compiler or not is_descendant(process_id, ancestor_id, proc_root):
            continue
        # MSBuild normally passes the compiler options through a response file.
        options = list(arguments)
        for argument in arguments:
            if argument.startswith('@'):
                response_file = directory / 'cwd' / argument[1:].strip('"')
                try:
                    options.append(response_file.read_text(encoding='utf-8-sig'))
                except (OSError, UnicodeError):
                    continue
        outputs = re.findall(r'(?:^|\s)/out:(?:"([^"]+)"|(\S+))', '\n'.join(options))
        if any(Path(quoted or unquoted).name == project + '.dll' for quoted, unquoted in outputs):
            yield process_id


def stop_owned_process(process):
    if process is None or process.poll() is not None:
        return
    try:
        os.killpg(process.pid, signal.SIGTERM)
        process.wait(timeout=5)
    except subprocess.TimeoutExpired:
        os.killpg(process.pid, signal.SIGKILL)
        process.wait()
    except ProcessLookupError:
        pass


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--project', required=True)
    parser.add_argument('--trace-tool', required=True)
    parser.add_argument('--output-directory', type=Path, required=True)
    parser.add_argument('command', nargs=argparse.REMAINDER)
    args = parser.parse_args()
    command = args.command[1:] if args.command[:1] == ['--'] else args.command
    if not command or platform.system() != 'Linux':
        parser.error('A command and Linux /proc are required.')

    def interrupted(signum, _frame):
        raise SystemExit(128 + signum)

    signal.signal(signal.SIGTERM, interrupted)
    trace = None
    trace_log = None
    build = None
    first_seen = {}
    sampled = False
    trace_started = 0
    output = args.output_directory / 'PublicAPI.compiler'
    try:
        build = subprocess.Popen(command, start_new_session=True)
        while build.poll() is None:
            now = time.monotonic()
            if trace is not None and trace.poll() is None and now - trace_started >= 90:
                stop_owned_process(trace)
            if not sampled:
                compilers = list(find_compilers(build.pid, args.project))
                first_seen = {process_id: first_seen.get(process_id, now) for process_id in compilers}
                for process_id in compilers:
                    if now - first_seen[process_id] < 120:
                        continue
                    trace_command = [args.trace_tool, 'collect', '--process-id', str(process_id),
                                     '--duration', '00:00:30', '--buffersize', '64',
                                     '--profile', 'dotnet-common,dotnet-sampled-thread-time',
                                     '--output', str(output) + '.nettrace']
                    metadata = {'project': args.project, 'processId': process_id,
                                'utc': datetime.now(timezone.utc).isoformat(),
                                'platform': platform.platform(), 'commit': os.getenv('GITHUB_SHA'),
                                'command': trace_command}
                    try:
                        metadata['status'] = Path(f'/proc/{process_id}/status').read_text()
                        metadata['stat'] = Path(f'/proc/{process_id}/stat').read_text()
                        Path(str(output) + '.json').write_text(json.dumps(metadata, indent=2))
                        trace_log = open(str(output) + '.log', 'w')
                        trace = subprocess.Popen(trace_command, stdout=trace_log,
                                                 stderr=subprocess.STDOUT, start_new_session=True)
                        sampled = True
                        trace_started = time.monotonic()
                        print(f'Collecting 30 seconds of compiler diagnostics for PID {process_id}.',
                              flush=True)
                    except OSError as error:
                        if trace_log is not None:
                            trace_log.close()
                            trace_log = None
                        print(f'Compiler diagnostics unavailable: {error}', flush=True)
                    break
            time.sleep(1)
        return build.returncode
    finally:
        stop_owned_process(trace)
        stop_owned_process(build)
        if trace_log is not None:
            trace_log.close()


if __name__ == '__main__':
    raise SystemExit(main())
