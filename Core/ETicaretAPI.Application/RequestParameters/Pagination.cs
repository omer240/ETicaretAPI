using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.RequestParameters
{
    public class Pagination
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;

        [BindNever]
        public int Skip => (Page - 1) * Size;
    }
}
