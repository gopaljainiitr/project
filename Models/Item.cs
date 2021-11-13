using System;
using System.Collections.Generic;
using System.Text;

namespace Galytix.WebApi.Models
{
    public class Item
    {
        public long Id { get; set; }
        public string? Country { get; set; }
        public string[] Lob { get; set; }
    }
}
