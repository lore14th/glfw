using Sharpmake;
using System;
using System.Collections.Generic;
using System.IO;

[Sharpmake.Generate]
public class GLFW : Sharpmake.Project
{
	public GLFW()
	{
		Name = "GLFW";
		IsFileNameToLower = false;
		IsTargetFileNameToLower = false;

		SourceFiles.Add("GLFW.Build.cs");

		AddTargets(new Target(
			Platform.win64,
			DevEnv.vs2022,
			Optimization.Debug | Optimization.Release)
		);
	}

	[Sharpmake.Configure]
	public void ConfigureAll(Project.Configuration Config, Target Target)
	{
		Config.Output = Configuration.OutputType.Lib;
		Config.ProjectPath = TinfoilSolution.GetProjectFileFolder();
		Config.TargetPath = TinfoilSolution.GetBinaryFolder(Name, Target);
		Config.IntermediatePath = TinfoilSolution.GetIntermediateFolder(Name, Target);

		Config.Options.Add(Options.Vc.Compiler.CLanguageStandard.C11);
		Config.Options.Add(Options.Vc.General.WindowsTargetPlatformVersion.Latest);
		Config.Options.Add(Options.Vc.Librarian.TreatLibWarningAsErrors.Enable);
		Config.Options.Add(new Options.Vc.Compiler.DisableSpecificWarnings("4244", "4100"));
		// C4244: Cast possible data loss
		// C4100: unused function param

		Config.IncludePaths.Add("/include");

		Config.Defines.Add("_CRT_SECURE_NO_WARNINGS");

		List<string> ExcludedFilePrefixes = new List<string>();
		if (Target.Platform == Platform.win64)
		{
			Config.Defines.Add("_GLFW_WIN32");

			ExcludedFilePrefixes.Add("cocoa");
			ExcludedFilePrefixes.Add("linux");
			ExcludedFilePrefixes.Add("posix");
			ExcludedFilePrefixes.Add("wl");
			ExcludedFilePrefixes.Add("x11");
			ExcludedFilePrefixes.Add("xkb");
			ExcludedFilePrefixes.Add("glx");
		}

		List<string> ExcludedFolders = new List<string>();
		ExcludedFolders.Add("deps");
		ExcludedFolders.Add("examples");
		ExcludedFolders.Add("tests");
			
		Config.SourceFilesBuildExcludeRegex.Add(@".*" + string.Join("|", ExcludedFilePrefixes.ToArray()) + @".*");
		Config.SourceFilesBuildExcludeRegex.Add(@"\.*\/(" + string.Join("|", ExcludedFolders.ToArray()) + @")\/");
	}

	public static void LinkToProject(Project.Configuration ParentConfig, ITarget Target, string Folder, DependencySetting DependencySetting = DependencySetting.Default)
	{
		ParentConfig.AddPrivateDependency<GLFW>(Target, DependencySetting, Folder);
		ParentConfig.IncludePaths.Add("/include");
	}
}
