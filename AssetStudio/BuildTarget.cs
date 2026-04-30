using System;

namespace AssetStudio
{
    /// <summary>
    /// Unity build target platform enumeration.
    /// </summary>
    public enum BuildTarget
    {
        NoTarget = -2,
        AnyPlayer = -1,
        ValidPlayer = 1,
        /// <summary>
        /// Universal macOS standalone
        /// </summary>
        StandaloneOSX = 2,
        /// <summary>
        /// macOS standalone (PowerPC only)
        /// </summary>
        StandaloneOSXPPC = 3,
        /// <summary>
        /// macOS standalone (Intel only)
        /// </summary>
        StandaloneOSXIntel = 4,
        /// <summary>
        /// Windows standalone
        /// </summary>
        StandaloneWindows = 5,
        /// <summary>
        /// Web player (LZMA)
        /// </summary>
        WebPlayer = 6,
        /// <summary>
        /// Streamed web player
        /// </summary>
        WebPlayerStreamed = 7,
        /// <summary>
        /// Nintendo Wii
        /// </summary>
        Wii = 8,
        /// <summary>
        /// iOS player
        /// </summary>
        iOS = 9,
        /// <summary>
        /// PlayStation 3
        /// </summary>
        PS3 = 10,
        /// <summary>
        /// Xbox 360
        /// </summary>
        XBOX360 = 11,
        Broadcom = 12,
        /// <summary>
        /// Android .apk standalone app
        /// </summary>
        Android = 13,
        StandaloneGLESEmu = 14,
        StandaloneGLES20Emu = 15,
        /// <summary>
        /// Google Native Client
        /// </summary>
        NaCl = 16,
        /// <summary>
        /// Linux standalone
        /// </summary>
        StandaloneLinux = 17,
        FlashPlayer = 18,
        /// <summary>
        /// Windows 64-bit standalone
        /// </summary>
        StandaloneWindows64 = 19,
        /// <summary>
        /// WebGL
        /// </summary>
        WebGL = 20,
        /// <summary>
        /// Windows Store Apps (x86)
        /// </summary>
        WSAPlayerX86 = 21,
        /// <summary>
        /// Windows Store Apps (x64)
        /// </summary>
        WSAPlayerX64 = 22,
        /// <summary>
        /// Windows Store Apps (ARM)
        /// </summary>
        WSAPlayerARM = 23,
        /// <summary>
        /// Linux 64-bit standalone
        /// </summary>
        StandaloneLinux64 = 24,
        /// <summary>
        /// Linux universal standalone
        /// </summary>
        StandaloneLinuxUniversal = 25,
        /// <summary>
        /// Windows Phone 8 player
        /// </summary>
        WP8Player = 26,
        /// <summary>
        /// macOS Intel 64-bit standalone
        /// </summary>
        StandaloneOSXIntel64 = 27,
        /// <summary>
        /// BlackBerry
        /// </summary>
        BlackBerry = 28,
        /// <summary>
        /// Tizen player
        /// </summary>
        Tizen = 29,
        /// <summary>
        /// PS Vita
        /// </summary>
        PSP2 = 30,
        /// <summary>
        /// PlayStation 4
        /// </summary>
        PS4 = 31,
        /// <summary>
        /// PlayStation Mobile
        /// </summary>
        PSM = 32,
        /// <summary>
        /// Xbox One
        /// </summary>
        XboxOne = 33,
        /// <summary>
        /// Samsung Smart TV
        /// </summary>
        SamsungTV = 34,
        /// <summary>
        /// Nintendo 3DS
        /// </summary>
        N3DS = 35,
        /// <summary>
        /// Wii U
        /// </summary>
        WiiU = 36,
        /// <summary>
        /// Apple tvOS
        /// </summary>
        tvOS = 37,
        /// <summary>
        /// Nintendo Switch
        /// </summary>
        Switch = 38,
        /// <summary>
        /// Magic Leap Lumin
        /// </summary>
        Lumin = 39,
        /// <summary>
        /// Google Stadia
        /// </summary>
        Stadia = 40,
        CloudRendering = 41,
        /// <summary>
        /// Xbox Series X|S
        /// </summary>
        GameCoreXboxSeries = 42,
        /// <summary>
        /// Xbox One (GameCore)
        /// </summary>
        GameCoreXboxOne = 43,
        /// <summary>
        /// PlayStation 5
        /// </summary>
        PS5 = 44,
        EmbeddedLinux = 45,
        QNX = 46,
        /// <summary>
        /// Apple Vision OS
        /// </summary>
        VisionOS = 47,
        /// <summary>
        /// Nintendo Switch 2
        /// </summary>
        Switch2 = 48,
        Kepler = 49,

        UnknownPlatform = 9999
    }

    public static class BuildTargetExtensions
    {
        public static bool IsStandalone(this BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneLinux:
                case BuildTarget.StandaloneLinux64:
                case BuildTarget.StandaloneLinuxUniversal:
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXPPC:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsWindows(this BuildTarget target)
        {
            return target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64;
        }

        public static bool IsOSX(this BuildTarget target)
        {
            return target == BuildTarget.StandaloneOSX ||
                   target == BuildTarget.StandaloneOSXIntel ||
                   target == BuildTarget.StandaloneOSXIntel64 ||
                   target == BuildTarget.StandaloneOSXPPC;
        }

        public static bool IsLinux(this BuildTarget target)
        {
            return target == BuildTarget.StandaloneLinux ||
                   target == BuildTarget.StandaloneLinux64 ||
                   target == BuildTarget.StandaloneLinuxUniversal;
        }

        public static bool IsMobile(this BuildTarget target)
        {
            return target == BuildTarget.iOS ||
                   target == BuildTarget.Android ||
                   target == BuildTarget.WP8Player ||
                   target == BuildTarget.BlackBerry ||
                   target == BuildTarget.Tizen;
        }

        public static bool IsConsole(this BuildTarget target)
        {
            return target == BuildTarget.PS3 ||
                   target == BuildTarget.PS4 ||
                   target == BuildTarget.PS5 ||
                   target == BuildTarget.XBOX360 ||
                   target == BuildTarget.XboxOne ||
                   target == BuildTarget.GameCoreXboxSeries ||
                   target == BuildTarget.GameCoreXboxOne ||
                   target == BuildTarget.Wii ||
                   target == BuildTarget.WiiU ||
                   target == BuildTarget.Switch ||
                   target == BuildTarget.Switch2 ||
                   target == BuildTarget.PSP2 ||
                   target == BuildTarget.N3DS ||
                   target == BuildTarget.Stadia;
        }

        public static bool IsApplePlatform(this BuildTarget target)
        {
            return target == BuildTarget.iOS ||
                   target == BuildTarget.tvOS ||
                   target == BuildTarget.VisionOS ||
                   target.IsOSX();
        }
    }
}
