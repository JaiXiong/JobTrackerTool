using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Utils.Operations
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public List<string> Errors { get; set; } = new List<string>();

        public static OperationResult CreateSuccess(string message)
        {
            return new OperationResult { Success = true, Message = message };
        }

        public static OperationResult CreateFailure(string message)
        {
            return new OperationResult { Success = false, Message = message };
        }
    }
}
