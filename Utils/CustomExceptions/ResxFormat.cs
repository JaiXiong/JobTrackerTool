using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Utils.CustomExceptions
{
    public class ResxFormat
    {
        private ResourceManager _resourceManager;

        public ResxFormat(ResourceManager resourceManager) 
        {
            _resourceManager = resourceManager;
        }

        public string Create(string resxMessage)
        {
            return string.Format(_resourceManager.GetString(resxMessage));
        }

        public string Create(string resxMessage, string resxItem)
        {
            return string.Format(_resourceManager.GetString(resxMessage), resxItem);
        }
    }
}
