using Polyglot;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace ExtendedColorSchemes
{
    internal class Localizer : IInitializable
    {
        public void Initialize()
        {
            SiraUtil.Utilities.AssemblyFromPath("ExtendedColorSchemes.Resources.locales.csv", out Assembly assembly, out string path);
            string content = SiraUtil.Utilities.GetResourceContent(assembly, path);

            var asset = new LocalizationAsset
            {
                Format = GoogleDriveDownloadFormat.CSV,
                TextAsset = new TextAsset(content)
            };

            Localization.Instance.InputFiles.Add(asset);
            LocalizationImporter.Refresh();
        }
    }
}
