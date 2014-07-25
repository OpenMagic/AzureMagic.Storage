using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace AzureMagic.Storage.TableStorage.Specifications.Support
{
    public static class WindowAzureStorageEmulatorManager
    {
        private static readonly object ThisLock = new Object();

        private static ProcessStartInfo CreateProcessStartInfo(string arguments)
        {
            return new ProcessStartInfo
            {
                FileName = GetWAStorageEmulatorFileName(),
                Arguments = arguments,
                WindowStyle = ProcessWindowStyle.Hidden
            };
        }

        // ReSharper disable once InconsistentNaming
        private static string GetWAStorageEmulatorFileName()
        {
            var possibleDirectories = new List<string>(new[]
            {
                @"C:\Program Files (x86)\Microsoft SDKs\Windows Azure\Storage Emulator",
                @"C:\Program Files\Microsoft SDKs\Windows Azure\Storage Emulator"
            });

            var possibleFileNames = possibleDirectories.Select(possibleDirectory => Path.Combine(possibleDirectory, "WAStorageEmulator.exe"));
            var fileName = possibleFileNames.FirstOrDefault(File.Exists);

            if (fileName != null)
            {
                return fileName;
            }

            var message = string.Format(
                "Cannot find WAStorageEmulator.exe in {0}.",
                string.Join(" or ", possibleDirectories));

            throw new FileNotFoundException(message);
        }

        public static void StartEmulator()
        {
            lock (ThisLock)
            {
                if (IsEmulatorRunning())
                {
                    return;
                }

                using (var process = Process.Start(CreateProcessStartInfo("start")))
                {
                    if (process == null)
                    {
                        throw new Exception("Cannot start Windows Azure Storage Emulator.");
                    }
                }
            }
        }

        public static bool IsEmulatorRunning()
        {
            lock (ThisLock)
            {
                var request = WebRequest.Create("http://127.0.0.1:10002/");

                try
                {
                    request.GetResponse();
                    return true;
                }
                catch (WebException exception)
                {
                    switch (exception.Status)
                    {
                        case WebExceptionStatus.ProtocolError:
                            return true;

                        case WebExceptionStatus.ConnectFailure:
                            return false;

                        default:
                            throw new Exception(string.Format("Expected status to be {0} or {1} but found {2}.", WebExceptionStatus.ProtocolError, WebExceptionStatus.ConnectFailure, exception.Status));
                    }
                }
            }
        }

        public static void StopEmulator()
        {
            lock (ThisLock)
            {
                if (!IsEmulatorRunning())
                {
                    return;
                }

                using (var process = Process.Start(CreateProcessStartInfo("stop")))
                {
                    if (process == null)
                    {
                        throw new Exception("Cannot stop Windows Azure Storage Emulator.");
                    }
                }
            }
        }
    }
}