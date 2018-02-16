using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kros.TroubleShooterServer.Database
{
    public class ServisContext : DbContext
    {
        public ServisContext(DbContextOptions<ServisContext> options)
            : base(options)
        { }

        public DbSet<Servis> Servises { get; set; }
        public DbSet<ServisInformation> ServisInformations { get; set; }
    }

    [Table("Servis")]
    public class Servis
    {
        public long ServisId { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string AttachmentsDirectory { get; set; }

        public List<ServisInformation> ServisInformations { get; set; }
    }

    [Table("ServisInformation")]
    public class ServisInformation
    {
        public long ServisInformationId { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }

        public long ServisId { get; set; }
        public Servis Servis { get; set; }
    }
}
