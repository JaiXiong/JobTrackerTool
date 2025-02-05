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
            var message = _resourceManager.GetString(resxMessage);
            if (message == null)
            {
                throw new ArgumentNullException(nameof(resxMessage), "Resource string cannot be null.");
            }
            return string.Format(message);
        }

        public string Create(string resxMessage, string resxItem)
        {
            var message = _resourceManager.GetString(resxMessage);
            if (message == null)
            {
                throw new ArgumentNullException(nameof(resxMessage), "Resource string cannot be null.");
            }
            return string.Format(message, resxItem);
        }
    }
}
