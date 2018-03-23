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
    [ContextClass("ПолитикаКэшированияЭлемента", "CacheItemPolicy")]
    public class CacheItemPolicyImpl : AutoContext<CacheItemPolicyImpl>
    {
        CacheItemPolicy _policy;
        public CacheItemPolicy Policy
        {
            get
            {
                return _policy;
            }
        }
        [ScriptConstructor(Name = "Без параметров")]
        public static IRuntimeContextInstance Constructor()
        {
            return new CacheItemPolicyImpl();
        }

        public CacheItemPolicyImpl()
        {
            _policy = new CacheItemPolicy();
        }

        [ContextProperty("ДатаИстечения", "AbsoluteExpiration")]
        public IValue AbsoluteExpiration
        {
            get
            {
                if (_policy.AbsoluteExpiration == ObjectCache.InfiniteAbsoluteExpiration)
                    return ValueFactory.Create();
                else
                    return ValueFactory.Create(_policy.AbsoluteExpiration.UtcDateTime.ToLocalTime());
            }
            set
            {
                if (value == ValueFactory.Create() || value == null)
                    _policy.AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
                else
                    _policy.AbsoluteExpiration = new DateTimeOffset((value.AsDate()).ToUniversalTime());
            }
        }

        [ContextProperty("ПериодНеИспользования", "SlidingExpiration")]
        public IValue SlidingExpiration
        {
            get
            {
                if (_policy.SlidingExpiration == ObjectCache.NoSlidingExpiration)
                    return ValueFactory.Create();
                else
                    return ValueFactory.Create(_policy.SlidingExpiration.Seconds);
            }
            set
            {
                if (value == null || value == ValueFactory.Create())
                    _policy.SlidingExpiration = ObjectCache.NoSlidingExpiration;
                else
                    _policy.SlidingExpiration = new TimeSpan(0, 0, (int)(value.AsNumber()));
            }
        }

        [ContextProperty("МониторыИзменения", "ChangeMonitors")]
        public ArrayImpl ChangeMonitors
        {
            get
            {
                ArrayImpl monitors = new ArrayImpl();
                foreach (ChangeMonitor ci in _policy.ChangeMonitors)
                    monitors.Add(new HostFileChangeMonitorImpl((HostFileChangeMonitor)ci));
                return monitors;
            }
            set
            {
                _policy.ChangeMonitors.Clear();
                foreach (IValue ci in value)
                    _policy.ChangeMonitors.Add(((HostFileChangeMonitorImpl)ci).Monitor);
            }
        }
    }
}
