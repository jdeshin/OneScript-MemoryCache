// (c) Yury Deshin 2018
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.HostedScript.Library;

namespace OneScript.DefaultMemoryCache
{
    [ContextClass("МониторИзмененияФайловНаКомпьютере", "HostFileChangeMonitor")]
    public class HostFileChangeMonitorImpl : AutoContext<HostFileChangeMonitorImpl>
    {
        HostFileChangeMonitor _monitor;
        public HostFileChangeMonitor Monitor
        {
            get
            {
                return _monitor;
            }
        }


        [ScriptConstructor(Name = "По списку файлов")]
        public static IRuntimeContextInstance Constructor(ArrayImpl files)
        {
            return new HostFileChangeMonitorImpl(files);
        }

        public HostFileChangeMonitorImpl(ArrayImpl files)
        {
            List<string> filesList = new List<string>();
            foreach(IValue cv in files)
            {
                filesList.Add(cv.AsString());
            }
            _monitor = new HostFileChangeMonitor(filesList);
        }

        public HostFileChangeMonitorImpl(HostFileChangeMonitor monitor)
        {
            _monitor = monitor;
        }

        [ContextProperty("Файлы", "FilePaths")]
        public ArrayImpl FilePaths
        {
            get
            {
                ArrayImpl filePaths = new ArrayImpl();
                foreach (string ci in _monitor.FilePaths)
                    filePaths.Add(ValueFactory.Create(ci));

                return filePaths;
            }
        }

        [ContextProperty("БылиИзменения", "HasChanged")]
        public bool HasChanged
        {
            get
            {
                return _monitor.HasChanged;
            }
        }

        [ContextProperty("ДатаИзменения", "LastModified")]
        public DateTime LastModified
        {
            get
            {
                return _monitor.LastModified.UtcDateTime.ToLocalTime();
            }
        }
    }
}
