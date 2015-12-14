#pragma warning disable 0436

using System.Reflection;

[assembly: AssemblyCompany ("Schematron.NET")]
[assembly: AssemblyProduct ("Schematron.NET")]
[assembly: AssemblyCopyright ("Copyright © Daniel Cazzulino 2015")]
[assembly: AssemblyTrademark ("")]
[assembly: AssemblyCulture ("")]

#if DEBUG
[assembly: AssemblyConfiguration ("DEBUG")]
#else
[assembly: AssemblyConfiguration ("RELEASE")]
#endif

// AssemblyVersion = full version info, since it's used to determine agents versions
[assembly: AssemblyVersion (ThisAssembly.Version)]
// FileVersion = release-like simple version (i.e. 3.11.2 for cycle 5, SR2).
[assembly: AssemblyFileVersion (ThisAssembly.SimpleVersion)]
// InformationalVersion = full version + branch + commit sha.
[assembly: AssemblyInformationalVersion (ThisAssembly.InformationalVersion)]

partial class ThisAssembly
{
	/// <summary>
	/// Simple release-like version number, like 4.0.1 for a cycle 5, SR1 build.
	/// </summary>
	public const string SimpleVersion = Git.BaseVersion.Major + "." + Git.BaseVersion.Minor + "." + Git.BaseVersion.Patch;

	/// <summary>
	/// Full version, including commits since base version file, like 4.0.1.598
	/// </summary>
	public const string Version = SimpleVersion + "." + Git.Commits;

	/// <summary>
	/// Full version, plus branch and commit short sha.
	/// </summary>
	public const string InformationalVersion = Version + "-" + Git.Branch + "+" + Git.Commit;
}

#pragma warning restore 0436