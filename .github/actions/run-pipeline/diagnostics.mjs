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
export function forwardOutput(source, sink, child) {
  let grouped = false;
  let line = '';
  let pending;
  let exited = false;
  let ended = source.readableEnded || source.destroyed;
  let backpressured = false;
  let cleanupTimer;
  let remainingForwardingTime = 30_000;
  let forwardingSince;
  function resetCleanup() {
    clearTimeout(cleanupTimer);
    const now = Date.now();
    if (forwardingSince !== undefined) {
      remainingForwardingTime -= Math.max(0, now - forwardingSince);
      forwardingSince = undefined;
    }
    // A descendant may hold stdout open, but output waiting on the sink is not idle.
    if (exited && !ended && !backpressured) {
      // Keep a total forwarding budget as well as the idle deadline: a descendant
      // writing continuously must not keep the action alive. Backpressure spends neither.
      forwardingSince = now;
      cleanupTimer = setTimeout(() => source.destroy(), Math.max(0, Math.min(5000, remainingForwardingTime)));
      cleanupTimer.unref();
    }
  }
  child?.once('exit', () => { exited = true; resetCleanup(); });
  function finish() { ended = true; clearTimeout(cleanupTimer); }
  source.once('end', finish);
  source.once('close', finish);
  function forward(chunk) {
    if (!sink.write(chunk) && !backpressured) {
      backpressured = true;
      source.pause();
      sink.once('drain', () => {
        backpressured = false;
        resetCleanup();
        source.resume();
      });
    }
    resetCleanup();
  }
  source.on('data', chunk => {
    forward(chunk);
    const lines = (line + chunk.toString('latin1')).split('\n');
    // Only the start of a line identifies a workflow command.
    line = lines.pop().slice(0, 16);
    for (const complete of lines) {
      if (complete.startsWith('::group::')) grouped = true;
      else if (complete.startsWith('::endgroup::')) grouped = false;
    }
    if (pending && !grouped && !line) {
      forward(pending);
      pending = undefined;
    }
  });
  return text => {
    if (grouped || line) pending = `${text}\n`;
    else forward(`${text}\n`);
  };
}

export function startDiagnostics(environment, child, {
  capture = captureDiagnostics, write, stdout = process.stdout, schedule = setInterval, cancel = clearInterval,
} = {}) {
  if (!samplesResources(environment)) {
    return;
  }

  if (!write) {
    write = forwardOutput(child.stdout, stdout, child);
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
