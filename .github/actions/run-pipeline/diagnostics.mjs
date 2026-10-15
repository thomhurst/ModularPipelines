import { execFile } from 'node:child_process';
import { readFile } from 'node:fs/promises';
import { promisify } from 'node:util';

const execute = promisify(execFile);

export async function captureDiagnostics(run = execute, read = readFile) {
  const [{ stdout }, memory] = await Promise.all([
    run('ps', ['-eo', 'pid,ppid,comm,pcpu,rss,stat,wchan:24', '--sort=-rss'], {
      timeout: 5000, maxBuffer: 1024 * 1024,
    }),
    read('/proc/meminfo', 'utf8'),
  ]);
  const counters = memory.split('\n').filter(line =>
    /^(MemTotal|MemAvailable|SwapTotal|SwapFree|Dirty|Writeback):/.test(line));
  // Do not collect command arguments or environment variables: they can contain credentials.
  return [
    `Pipeline resource sample ${new Date().toISOString()}`,
    ...counters,
    ...stdout.trimEnd().split('\n').slice(0, 21),
  ].join('\n');
}

export function startDiagnostics(environment, child, {
  capture = captureDiagnostics, write = console.log, schedule = setInterval, cancel = clearInterval,
} = {}) {
  if (environment.RUNNER_OS !== 'Linux' || environment.MODULARPIPELINES_INSTANCE_INDEX !== '0') {
    return;
  }

  let stopped = false;
  let sampling = false;
  async function sample() {
    if (stopped || sampling) return;
    sampling = true;
    try {
      const snapshot = await capture();
      if (!stopped) write(snapshot);
    } catch {
      if (!stopped) write('Pipeline resource sample unavailable.');
    } finally {
      sampling = false;
    }
  }

  const timer = schedule(sample, 60_000);
  timer.unref();
  function stop() {
    stopped = true;
    cancel(timer);
  }
  child.once('exit', stop);
  child.once('error', stop);
  void sample();
}
