using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace OcuFix.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        public virtual bool DisableASW { get; set; } = true;
        public virtual bool ServerPriority { get; set; } = true;
        public virtual bool GamePriority { get; set; } = true;
        public virtual string DebugToolPath { get; set; } = @"C:\Program Files\Oculus\Support\oculus-diagnostics\OculusDebugToolCLI.exe";

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