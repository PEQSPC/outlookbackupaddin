using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupAddIn.Models
{
    public class Registo
    {

        public Guid RegistoID { get; set; }
        public string Sigla { get; set; }

        public string PCName { get; set; }

        public string LastBackupDate { get; set; }
    }
}
