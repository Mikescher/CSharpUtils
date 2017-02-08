using System.Diagnostics;
using System.Text;

namespace MSHC.Util.Helper
{
	public struct ProcessOutput
	{
		public readonly int ExitCode;
		public readonly string StdOut;
		public readonly string StdErr;

		public ProcessOutput(int ex, string stdout, string stderr)
		{
			ExitCode = ex;
			StdOut = stdout;
			StdErr = stderr;
		}
	}

	public static class ProcessHelper
	{
		public static ProcessOutput ProcExecute(string command, string arguments, string workingDirectory = null)
		{
			Process process = new Process
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
					ErrorDialog = false
				}
			};

			StringBuilder builderOut = new StringBuilder();
			StringBuilder builderErr = new StringBuilder();

			process.OutputDataReceived += (sender, args) =>
			{
				if (args.Data == null) return;

				if (builderOut.Length == 0)
					builderOut.Append(args.Data);
				else
					builderOut.Append("\n" + args.Data);
			};

			process.ErrorDataReceived += (sender, args) =>
			{
				if (args.Data == null) return;

				if (builderErr.Length == 0)
					builderErr.Append(args.Data);
				else
					builderErr.Append("\n" + args.Data);
			};

			process.Start();

			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			
			process.WaitForExit();

			return new ProcessOutput(process.ExitCode, builderOut.ToString(), builderErr.ToString());
		}
	}
}
