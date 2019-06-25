using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication15.Helpers
{
    public class AppSettings
    {
        public String Secret { get; set; }
        public int AlowedPicUpLoadNum { get; set; }
        public string[] Catalogue { get; set; }
    }
}
