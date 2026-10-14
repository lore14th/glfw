// Tinfoil Engine Build Script

using Sharpmake;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TinfoilBuildTool;

using Configuration = Sharpmake.Project.Configuration;

[BuildModule(typeof(TinfoilProject))]
public class GLFW : TinfoilThirdPartyModuleDependency
{
	public GLFW(ReadOnlyModuleRules rules) : base(rules)
	{
	}

	public override void ConfigureBuild(Configuration config, ITarget target)
	{
		base.ConfigureBuild(config, target);

		config.Options.Add(Options.Vc.Compiler.CLanguageStandard.C11);
		config.Options.Add(Options.Vc.General.WindowsTargetPlatformVersion.Latest);
		config.Options.Add(Options.Vc.Librarian.TreatLibWarningAsErrors.Enable);
		// C4244: Cast possible data loss, C4100: unused function param
		config.Options.Add(new Options.Vc.Compiler.DisableSpecificWarnings("4244", "4100"));

		config.IncludePaths.Add(Path.Combine("[project.SharpmakeCsPath]", "include"));
		config.Defines.Add("_CRT_SECURE_NO_WARNINGS");

		List<string> platformsToRemove = new List<string>();
		if (target.GetPlatform() == Platform.win32 || target.GetPlatform() == Platform.win64)
		{
			config.Defines.Add("_GLFW_WIN32");
			platformsToRemove.AddRange(new string[] { "cocoa", "glx", "linux", "posix", "wl", "x11", "xkb" });

		}

		FilterSourceFiles(config, platformsToRemove);
	}

	private void FilterSourceFiles(Configuration config, List<string> platformToRemove)
	{
		config.SourceFilesBuildExclude.AddRange(Directory.EnumerateFiles(Path.Combine(RootDirectory, "deps"), "*", SearchOption.AllDirectories));
		config.SourceFilesBuildExclude.AddRange(Directory.EnumerateFiles(Path.Combine(RootDirectory, "examples"), "*", SearchOption.AllDirectories));
		config.SourceFilesBuildExclude.AddRange(Directory.EnumerateFiles(Path.Combine(RootDirectory, "tests"), "*", SearchOption.AllDirectories));

		config.SourceFilesBuildExclude.AddRange(Directory.EnumerateFiles(Path.Combine(RootDirectory, "src"), "*", SearchOption.AllDirectories).
			Where(file => platformToRemove.Any(platform => Path.GetFileName(file).StartsWith(platform, StringComparison.OrdinalIgnoreCase))));
	}
}
