using System;
using System.Collections.Generic;
using System.IO;

namespace AssetStudio
{
    internal static class ResourceFileNameHelper
    {
        private const string ArchivePrefix = "archive:/";
        private const string LibraryFolder = "library/";
        private const string ResourcesFolder = "resources/";
        private const string DefaultResourceName1 = "unity default resources";
        private const string DefaultResourceName2 = "unity_default_resources";
        private const string BuiltinExtraName1 = "unity builtin extra";
        private const string BuiltinExtraName2 = "unity_builtin_extra";

        public static IEnumerable<string> GetLookupNames(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                yield break;
            }

            foreach (var candidate in GetNameVariants(name))
            {
                yield return candidate;

                var fileName = Path.GetFileName(candidate);
                if (!string.IsNullOrEmpty(fileName) && !StringComparer.OrdinalIgnoreCase.Equals(candidate, fileName))
                {
                    yield return fileName;
                }
            }
        }

        public static IEnumerable<string> GetRegistrationNames(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                yield break;
            }

            foreach (var candidate in GetNameVariants(name))
            {
                yield return candidate;
            }
        }

        public static string FixFileIdentifier(string name)
        {
            name = name.Replace('\\', '/').ToLowerInvariant();
            name = FixDependencyName(name);
            return FixResourcePath(name);
        }

        private static IEnumerable<string> GetNameVariants(string name)
        {
            yield return name;

            var fixedName = FixFileIdentifier(name);
            if (!StringComparer.OrdinalIgnoreCase.Equals(name, fixedName))
            {
                yield return fixedName;
            }

            switch (fixedName)
            {
                case DefaultResourceName1:
                    yield return DefaultResourceName2;
                    break;
                case DefaultResourceName2:
                    yield return DefaultResourceName1;
                    break;
                case BuiltinExtraName1:
                    yield return BuiltinExtraName2;
                    break;
                case BuiltinExtraName2:
                    yield return BuiltinExtraName1;
                    break;
            }
        }

        private static string FixDependencyName(string name)
        {
            if (name.StartsWith(LibraryFolder, StringComparison.Ordinal))
            {
                return name.Substring(LibraryFolder.Length);
            }
            if (name.StartsWith(ResourcesFolder, StringComparison.Ordinal))
            {
                return name.Substring(ResourcesFolder.Length);
            }
            return name;
        }

        private static string FixResourcePath(string resourcePath)
        {
            return resourcePath.StartsWith(ArchivePrefix, StringComparison.Ordinal)
                ? Path.GetFileName(resourcePath)
                : resourcePath;
        }
    }
}
