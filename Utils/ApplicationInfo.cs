using System.Reflection;
using System;

namespace AvaloniaDesktop.Utils;

public sealed class ApplicationInfo : IApplicationInfo
{
    public string Name { get; }
    public string Organization { get; }
    public string Version { get; }
    public string FileVersion { get; }
    public DateTime Created { get; }

    public ApplicationInfo(Assembly assembly)
    {
        Name = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? "no data";
        Organization = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? "no data";
        Version = assembly.GetName().Version?.ToString() ?? "0.0.0.0";
        FileVersion = assembly.GetName().Version?.ToString() ?? "0.0.0.0";
        Created = DateTime.Now;
    }
}

