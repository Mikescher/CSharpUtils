using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace MSHC.Util.Helper
{
	public struct ProcessOutput
	{
		public readonly string Command;
		public readonly int ExitCode;
		public readonly string StdOut;
		public readonly string StdErr;
		public readonly string StdCombined;
		
		public ProcessOutput(string cmd, int ex, string stdout, string stderr, string stdcom)
		{
			Command = cmd;
			ExitCode = ex;
			StdOut = stdout;
			StdErr = stderr;
			StdCombined = stdcom;
		}

		public override string ToString() => $"{Command}\n=> {ExitCode}\n\n[stdout]\n{StdOut}\n\n[stderr]\n{StdErr}";
	}
	
	public enum ProcessHelperStream
    {
		StdOut,
		StdErr,
    }

	public static class ProcessHelper
	{
		private static Regex REX_LINE = new Regex(@"\r?\n", RegexOptions.Compiled);

		public static ProcessOutput ProcExecute(string command, string arguments, string workingDirectory = null, Action<ProcessHelperStream, string> listener = null)
		{
			var process = new Process
			{
				StartInfo =
				{
					FileName = command,
					Arguments = arguments,
					WorkingDirectory = workingDirectory ?? string.Empty,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true,
					ErrorDialog = false,
				}
			};

			var builderOut = new StringBuilder();
			var builderErr = new StringBuilder();
			var builderBoth = new StringBuilder();

			process.OutputDataReceived += (sender, args) =>
			{
				if (args.Data == null) return;

				if (builderOut.Length == 0) builderOut.Append(args.Data);
				else builderOut.Append("\n" + args.Data);

				if (builderBoth.Length == 0) builderBoth.Append(args.Data);
				else builderBoth.Append("\n" + args.Data);

				if (listener != null)
				{
					foreach (var line in REX_LINE.Split(args.Data)) listener(ProcessHelperStream.StdOut, args.Data);
				}
			};

			process.ErrorDataReceived += (sender, args) =>
			{
				if (args.Data == null) return;

				if (builderErr.Length == 0) builderErr.Append(args.Data);
				else builderErr.Append("\n" + args.Data);

				if (builderBoth.Length == 0) builderBoth.Append(args.Data);
				else builderBoth.Append("\n" + args.Data);

				if (listener != null)
				{
					foreach (var line in REX_LINE.Split(args.Data)) listener(ProcessHelperStream.StdErr, args.Data);
				}
			};

			process.Start();

			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

			process.WaitForExit();

			return new ProcessOutput($"{command} {arguments.Replace("\r", "\\r").Replace("\n", "\\n")}", process.ExitCode, builderOut.ToString(), builderErr.ToString(), builderBoth.ToString());
		}
	}
}
