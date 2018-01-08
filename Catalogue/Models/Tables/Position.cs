using Catalogue.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catalogue.Models
{
    public class Position
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}