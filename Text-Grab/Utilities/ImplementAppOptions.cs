using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Windows.ApplicationModel;

namespace Text_Grab.Utilities;

internal class ImplementAppOptions
{
    public static async Task ImplementStartupOption(bool startupOnLogin)
    {
        if (startupOnLogin)
            await SetForStartup();
        else
            RemoveFromStartup();
    }

    public static void ImplementBackgroundOption(bool runInBackground)
    {
        if (runInBackground)
        {
            // Get strongly-typed current application
            NotifyIconUtilities.SetupNotifyIcon();
        }
        else
        {
            App app = (App)Application.Current;
            if (app.TextGrabIcon != null)
            {
                app.TextGrabIcon.Dispose();
                app.TextGrabIcon = null;
            }
        }
    }

    private static async void RemoveFromStartup()
    {
        if (AppUtilities.IsPackaged())
        {
            StartupTask startupTask = await StartupTask.GetAsync("StartTextGrab");
            startupTask.Disable();
        }
        else
        {
            string path = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(path, true);
            if (key is not null)
            {
                try { key.DeleteValue("Text-Grab"); }
                catch (Exception) { }
            }
        }
    }

    private static async Task SetForStartup()
    {
        if (AppUtilities.IsPackaged())
        {
            StartupTask startupTask = await StartupTask.GetAsync("StartTextGrab");
            StartupTaskState newState = await startupTask.RequestEnableAsync();
        }
        else
        {
            string path = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            string? BaseDir = Path.GetDirectoryName(AppContext.BaseDirectory);
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(path, true);
            if (key is not null
                && BaseDir is not null)
            {
                key.SetValue("Text-Grab", $"\"{BaseDir}\\Text-Grab.exe\"");
            }
        }
        await Task.CompletedTask;
    }
}
