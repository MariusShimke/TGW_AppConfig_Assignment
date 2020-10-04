using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariusSimke_TGWassignment.DataModels.Abstraction
{
    interface IConfigData
    {
        void UpdateConfigKey(string strKey, string newValue);
        bool ConfigKeyExists(string strKey);
    }
}
