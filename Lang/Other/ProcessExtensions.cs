﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MSHC.Lang.Other
{
    public static class ProcessExtensions
    {
        public static async Task<bool> WaitForExitAsync(this Process process, int timeout, CancellationToken cancellationToken = default)
        {
            var task = WaitForExitAsync(process);
            return (await Task.WhenAny(task, Task.Delay(timeout)) == task);
        }

        public static async Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            void Process_Exited(object sender, EventArgs e)
            {
                tcs.TrySetResult(true);
            }

            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;

            try
            {
                if (process.HasExited)
                {
                    return;
                }

                using (cancellationToken.Register(() => tcs.TrySetCanceled()))
                {
                    await tcs.Task.ConfigureAwait(false);
                }
            }
            finally
            {
                process.Exited -= Process_Exited;
            }
        }
    }
}
