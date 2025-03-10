// GLFW Tinfoil build script

using Sharpmake;
using System;
using System.Collections.Generic;
using System.IO;

[Sharpmake.Generate]
public class GLFW : TinfoilProjectBase
{
	public GLFW()
	{
		Name = "GLFW";
		SourceFiles.Add("GLFW.Build.cs");
	}

	[Sharpmake.Configure]
	public void ConfigureAll(Project.Configuration config, Target target)
	{
		config.Output = Configuration.OutputType.Lib;

		config.Options.Add(Options.Vc.Compiler.CLanguageStandard.C11);
		config.Options.Add(Options.Vc.General.WindowsTargetPlatformVersion.Latest);
		config.Options.Add(Options.Vc.Librarian.TreatLibWarningAsErrors.Enable);
		// C4244: Cast possible data loss, C4100: unused function param
		config.Options.Add(new Options.Vc.Compiler.DisableSpecificWarnings("4244", "4100"));

		config.IncludePaths.Add("/include");
		config.Defines.Add("_CRT_SECURE_NO_WARNINGS");

		ExcludeFolder(config, target, "deps");
		ExcludeFolder(config, target, "examples");
		ExcludeFolder(config, target, "tests");

		if (target.Platform == Platform.win64)
		{
			config.Defines.Add("_GLFW_WIN32");

			ExcludeFileByPrefix(config, target, "cocoa");
			ExcludeFileByPrefix(config, target, "glx");
			ExcludeFileByPrefix(config, target, "linux");
			ExcludeFileByPrefix(config, target, "posix");
			ExcludeFileByPrefix(config, target, "wl");
			ExcludeFileByPrefix(config, target, "x11");
			ExcludeFileByPrefix(config, target, "xkb");
		}
	}
}
