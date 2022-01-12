using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace OcuFix.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        public virtual bool DisableASW { get; set; } = true;
        public virtual bool RestoreASW { get; set; } = true;
        public virtual bool SetPriority { get; set; } = true;
        public virtual bool RestorePriority { get; set; } = true;
        public virtual bool GamePriority { get; set; } = true;
        public virtual string DebugToolPath { get; set; } = @"C:\Program Files\Oculus\Support\oculus-diagnostics\OculusDebugToolCLI.exe";
        public virtual bool EnableChecks { get; set; } = true;

        public virtual void OnReload()
        {
            /**/
        }

        public virtual void Changed()
        {
            /**/
        }

        public virtual void CopyFrom(PluginConfig other)
        {
            /**/
        }
    }
}