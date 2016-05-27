using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator
{
    public abstract class Decorator : PluginInterface.IPlugin
    {
        protected PluginInterface.IPlugin regularPlugin;

        public virtual string extension
        {
            get;
        }
        public virtual string ToJson(string source)
        {           
            return regularPlugin.ToJson(source);           
        }
        public virtual string FromJson(string source)
        {         
            return regularPlugin.FromJson(source);            
        }               
        
    }
}
