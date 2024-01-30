﻿namespace CommonFramework.Runtime.UISystems
{
    /// <summary>
    /// Define UI types and paths in resource folder
    /// </summary>
    public class UIType
    {
        public string Path { get; private set; }

        public string Name { get; private set; }

        public UIType(string path)
        {
            Path = path;
            Name = path.Substring(path.LastIndexOf('/') + 1);
        }

        public override string ToString()
        {
            return string.Format("path : {0} name : {1}", Path, Name);
        }
    }
}
