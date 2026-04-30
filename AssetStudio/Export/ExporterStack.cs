using System;
using System.Collections.Generic;

namespace AssetStudio.Export
{
    /// <summary>
    /// Manages a collection of asset exporters.
    /// Allows registration and retrieval of exporters by asset type.
    /// </summary>
    public class ExporterStack
    {
        private readonly Dictionary<ClassIDType, IAssetExporter> _exporters = new Dictionary<ClassIDType, IAssetExporter>();
        private readonly List<IAssetExporter> _fallbackExporters = new List<IAssetExporter>();

        /// <summary>
        /// Registers an exporter for specific asset types.
        /// </summary>
        /// <param name="exporter">The exporter to register.</param>
        /// <param name="types">The asset types this exporter handles.</param>
        public void Register(IAssetExporter exporter, params ClassIDType[] types)
        {
            foreach (var type in types)
            {
                _exporters[type] = exporter;
            }
        }

        /// <summary>
        /// Registers a fallback exporter that can handle multiple types.
        /// </summary>
        /// <param name="exporter">The exporter to register as fallback.</param>
        public void RegisterFallback(IAssetExporter exporter)
        {
            _fallbackExporters.Add(exporter);
        }

        /// <summary>
        /// Overrides an existing exporter for a specific type.
        /// </summary>
        /// <param name="exporter">The new exporter.</param>
        /// <param name="type">The asset type to override.</param>
        public void Override(IAssetExporter exporter, ClassIDType type)
        {
            _exporters[type] = exporter;
        }

        /// <summary>
        /// Gets the exporter for the specified asset type.
        /// </summary>
        /// <param name="type">The asset type.</param>
        /// <returns>The exporter, or null if none found.</returns>
        public IAssetExporter GetExporter(ClassIDType type)
        {
            if (_exporters.TryGetValue(type, out var exporter))
            {
                return exporter;
            }

            foreach (var fallback in _fallbackExporters)
            {
                if (fallback.CanExport(type))
                {
                    return fallback;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if an exporter exists for the specified type.
        /// </summary>
        /// <param name="type">The asset type.</param>
        /// <returns>True if an exporter exists.</returns>
        public bool HasExporter(ClassIDType type)
        {
            return GetExporter(type) != null;
        }

        /// <summary>
        /// Exports an asset using the registered exporter.
        /// </summary>
        /// <param name="asset">The asset to export.</param>
        /// <param name="exportPath">The export directory.</param>
        /// <param name="options">Export options.</param>
        /// <returns>True if export was successful.</returns>
        public bool Export(Object asset, string exportPath, ExportOptions options)
        {
            var exporter = GetExporter(asset.type);
            if (exporter == null)
            {
                return false;
            }

            return exporter.Export(asset, exportPath, options);
        }
    }
}
