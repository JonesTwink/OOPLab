using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface
{
    public interface IPlugin
    {
        string extension {
                            get;
                         }
        string ToJson(string source);
        string FromJson(string source);
    }
}
