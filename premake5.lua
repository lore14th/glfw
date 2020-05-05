project "GLFW"
    kind "StaticLib"
    language "C"
    staticruntime "On"
    
	targetdir ("bin/" .. outputdir .. "/%{prj.name}")
	objdir ("bin-int/" .. outputdir .. "/%{prj.name}")

    files
    {
        "include/GLFW/glfw3.h",
        "include/GLFW/glfw3native.h",
        "src/glfw_config.h",
        "src/context.c",
        "src/init.c",
        "src/input.c",
        "src/monitor.c",
        "src/vulkan.c",
        "src/window.c",

        "src/internal.h",
        "src/mappings.h",
    }

    filter "system:windows"
        systemversion "latest"
        
        files
        {
            "src/win32_init.c",
            "src/win32_joystick.c",
            "src/win32_monitor.c",
            "src/win32_time.c",
            "src/win32_thread.c",
            "src/win32_window.c",
            "src/wgl_context.c",
            "src/egl_context.c",
            "src/osmesa_context.c"
        }

        defines 
        { 
            "_GLFW_WIN32",
            "_CRT_SECURE_NO_WARNINGS"
        }

    filter "system:macosx"
        systemversion "latest"
        staticruntime "On"
        pic "On"
        files 
        {
            "src/cocoa_init.m",
            "src/cocoa_joystick.m",
            "src/cocoa_monitor.m",
            "src/cocoa_window.m",
            "src/cocoa_time.c",
            "src/posix_thread.c",
            "src/nsgl_context.m",
            "src/egl_context.c",
            "src/osmesa_context.c"
        }

		links
		{
			"CoreFoundation.framework",
			"Cocoa.framework",
			"IOKit.framework",
			"CoreVideo.framework"
		}

        defines 
        { 
            "_GLFW_COCOA",
            "_CRT_SECURE_NO_WARNINGS"
        }
        
    filter "configurations:Debug"
        runtime "Debug"
        symbols "on"	-- debug version --

    filter "configurations:Release"
        runtime "Release"
        optimize "on"	-- release version --
