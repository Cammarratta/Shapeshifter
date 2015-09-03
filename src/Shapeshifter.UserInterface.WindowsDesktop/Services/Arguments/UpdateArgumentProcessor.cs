﻿using Shapeshifter.UserInterface.WindowsDesktop.Services.Arguments.Interfaces;
using Shapeshifter.UserInterface.WindowsDesktop.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using WindowsProcess = System.Diagnostics.Process;

namespace Shapeshifter.UserInterface.WindowsDesktop.Services.Arguments
{
    class UpdateArgumentProcessor : IArgumentProcessor
    {
        readonly IProcessManager processManager;

        public UpdateArgumentProcessor(
            IProcessManager processManager)
        {
            this.processManager = processManager;
        }

        public bool Terminates
            => true;

        public bool CanProcess(string[] arguments)
        {
            return arguments.Contains("update");
        }

        public void Process(string[] arguments)
        {
            var updateIndex = Array.IndexOf(arguments, "update");
            var targetDirectory = arguments[updateIndex + 1];

            var currentDirectory = Environment.CurrentDirectory;
            InstallNewVersion(targetDirectory, currentDirectory);
        }

        void InstallNewVersion(string targetDirectory, string currentDirectory)
        {
            foreach (var currentFile in Directory.GetFiles(currentDirectory))
            {
                HandleNewFile(targetDirectory, currentFile);
            }

            LaunchNewExecutable(targetDirectory);
        }

        void LaunchNewExecutable(string targetDirectory)
        {
            using (var currentProcess = WindowsProcess.GetCurrentProcess())
            {
                var executableFile = Path.Combine(targetDirectory, $"{currentProcess.ProcessName}.exe");
                processManager.LaunchFile(executableFile, $"cleanup {Environment.CurrentDirectory}");
            }
        }

        static void HandleNewFile(string targetDirectory, string currentFile)
        {
            var currentFileName = Path.GetFileName(currentFile);

            var targetFile = Path.Combine(targetDirectory, currentFileName);
            DeleteFileIfExists(targetFile);

            File.Copy(currentFile, targetFile);
        }

        static void DeleteFileIfExists(string targetFile)
        {
            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }
        }
    }
}