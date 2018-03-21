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
    [ContextClass("КэшПамятиПоУмолчанию", "DefaultMemoryCache")]
    public class DefaultMemoryCacheImpl : AutoContext<DefaultMemoryCacheImpl>
    {
        [ScriptConstructor(Name = "Без параметров")]
        public static IRuntimeContextInstance Constructor()
        {
            return new DefaultMemoryCacheImpl();
        }

        [ContextMethod("Добавить", "Add")]
        public bool Add(string key, IValue obj, CacheItemPolicyImpl policy)
        {
            return MemoryCache.Default.Add(key, obj, policy.Policy);
        }

        [ContextMethod("Получить", "Get")]
        public IValue Get(string key)
        {
            object result = MemoryCache.Default.Get(key);

            if (result == null)
                return ValueFactory.Create();

            return (IValue)result;
        }

        [ContextProperty("ОграничениеКэшаПамяти", "CacheMemoryLimit")]
        public long CacheMemoryLimit
        {
            get
            {
                return MemoryCache.Default.CacheMemoryLimit;
            }
        }

        [ContextProperty("ОграничениеФизическойПамяти", "PhysicalMemoryLimit")]
        public long PhysicalMemoryLimit
        {
            get
            {
                return MemoryCache.Default.PhysicalMemoryLimit;
            }
        }

    }
}
