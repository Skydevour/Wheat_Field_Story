#if UNITY_EDITOR

using System.IO;

namespace CommonFramework.Runtime.Global.Defines
{
    public static class EditorFilePaths
    {
        //Folder path defines
        public static readonly string UnityPackagesFolder = "Packages";
        public static readonly string PackageRoot = Path.Combine(UnityPackagesFolder, PackageName.Name);
        public static readonly string StyleSheetRoot = Path.Combine(PackageRoot, "StyleSheets");
        public static readonly string AssetEditorStyleSheetRoot = Path.Combine(StyleSheetRoot, "AssetEditor");

        //File Paths
        public static readonly string SelectAssetWindowStyleSheet = Path.Combine(AssetEditorStyleSheetRoot, "SelectAssetWindow.uss");
    }
}

#endif