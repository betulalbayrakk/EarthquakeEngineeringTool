using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapstoneProject1.Models
{
    public class Koordinat
    {
        public string dosyaAdi { get; set; }

        public string latitudeE { get; set; }

        public string longitudeE { get; set; }

        public string latitudeS { get; set; }

        public string longitudeS { get; set; }
        public string Distance1 { get; set; }
        public string Distance2 { get; set; }

        public string PGA1 { get; set; }
        public string PGA2 { get; set; }
        public string PGA3 { get; set; }
        public string PGA4 { get; set; }
        public string PGA5 { get; set; }

        public string PGV1 { get; set; }
        public string PGV2 { get; set; }
        public string PGV3 { get; set; }
        public string PGV4 { get; set; }
        public string PGV5 { get; set; }

        public string PGD1 { get; set; }
        public string PGD2 { get; set; }
        public string PGD3 { get; set; }
        public string PGD4 { get; set; }
        public string PGD5 { get; set; }

        public string RMSA1 { get; set; }

        public string RMSV1 { get; set; }

        public string RMSD1 { get; set; }

        public string ratio1 { get; set; }

        public string ArInt1 { get; set; }

        public string ChaInt1 { get; set; }
    }
}