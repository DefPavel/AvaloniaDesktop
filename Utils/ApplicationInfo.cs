using System.Reflection;
using System;

namespace AvaloniaDesktop.Utils;

public sealed class ApplicationInfo : IApplicationInfo
{
    public string Name { get; }
    public string Organization { get; }
    public string Copyright { get; }
    public string Version { get; }
    public string FileVersion { get; }
    public DateTime Created { get; }

    public ApplicationInfo(Assembly assembly)
    {
        Name = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
        Organization = assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
        Copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
        Version = assembly.GetName().Version.ToString();
        FileVersion = assembly.GetName().Version.ToString();
        Created = DateTime.Now;
    }
}

