using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common.Error
{
    public class ErrorBodyResponse
    {
        public string Title { get; set; } = "";
        public int StatusCode { get; set; } = 500;
        public string Detail { get; set; } = "";
    }
}
