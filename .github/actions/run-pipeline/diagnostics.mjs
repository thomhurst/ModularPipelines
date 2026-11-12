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
    `::group::Pipeline resource sample ${new Date().toISOString()}`,
    ...counters,
    ...stdout.trimEnd().split('\n').slice(0, 21),
    '::endgroup::',
  ].join('\n');
}

export function samplesResources(environment) {
  return environment.RUNNER_OS === 'Linux' && environment.MODULARPIPELINES_INSTANCE_INDEX === '0';
}

// Samples come from this process, outside the pipeline's output lock. GitHub log groups do not
// nest, so forward the pipeline's stdout and write a sample only at a line start outside a group.
export function forwardOutput(source, sink) {
  let grouped = false;
  let line = '';
  let pending;
  source.on('data', chunk => {
    if (!sink.write(chunk)) {
      source.pause();
      sink.once('drain', () => source.resume());
    }
    const lines = (line + chunk.toString('latin1')).split('\n');
    // Only the start of a line identifies a workflow command.
    line = lines.pop().slice(0, 16);
    for (const complete of lines) {
      if (complete.startsWith('::group::')) grouped = true;
      else if (complete.startsWith('::endgroup::')) grouped = false;
    }
    if (pending && !grouped && !line) {
      sink.write(pending);
      pending = undefined;
    }
  });
  return text => {
    if (grouped || line) pending = `${text}\n`;
    else sink.write(`${text}\n`);
  };
}

export function startDiagnostics(environment, child, {
  capture = captureDiagnostics, write, schedule = setInterval, cancel = clearInterval,
} = {}) {
  if (!samplesResources(environment)) {
    return;
  }

  if (!write) {
    write = forwardOutput(child.stdout, process.stdout);
    // A process that outlives the pipeline can hold the pipe open; do not wait for it.
    child.once('exit', () => setTimeout(() => child.stdout.destroy(), 5000).unref());
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
