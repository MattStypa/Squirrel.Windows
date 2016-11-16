using System;
using System.IO;
using System.Reflection;

namespace Squirrel.Update
{
    public class GovernorController
    {
        Type _governorType;
        protected object _governor;
        protected bool _silentInstall;
        BindingFlags _flags;

        public GovernorController(bool silentInstall)
        {
            _silentInstall = silentInstall;
            _flags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public;

            try {
                var source = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "governor.dll"
                );

                Assembly assembly = Assembly.LoadFile(source);
                _governorType = assembly.GetType("Governor");
                _governor = Activator.CreateInstance(_governorType);
            } catch { }
        }

        public bool Before()
        {
            try {
                return (bool)_governorType.InvokeMember("Before", _flags, null, _governor, new object[] { _silentInstall });
            } catch { }

            return true;
        }

        public void After()
        {
            try {
                _governorType.InvokeMember("After", _flags, null, _governor, new object[] { _silentInstall });
            } catch { }
        }
    }
}