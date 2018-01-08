using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catalogue.Models.Tables
{
    public class Administration
    {
        public int AdministrationId { get; set; }
        public string AdministrationName { get; set; }
        public int AdministrationPost { get; set; }
        public string AdministrationAddress { get; set; }
        public string AdministrationFax { get; set; }
        public string AdministrationEMail{ get; set; }
        public string AdministrationSkype { get; set; }
        public int AdministrationCode { get; set; }

        public int DepartmentId { get; set; }

    }
}