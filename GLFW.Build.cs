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
	public void ConfigureAll(Project.Configuration config, TinfoilTarget target)
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

			ExcludeFilesByPrefix(config, target, "cocoa");
			ExcludeFilesByPrefix(config, target, "glx");
			ExcludeFilesByPrefix(config, target, "linux");
			ExcludeFilesByPrefix(config, target, "posix");
			ExcludeFilesByPrefix(config, target, "wl");
			ExcludeFilesByPrefix(config, target, "x11");
			ExcludeFilesByPrefix(config, target, "xkb");
		}
	}
}
