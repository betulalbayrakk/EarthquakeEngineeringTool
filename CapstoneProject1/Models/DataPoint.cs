using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Web;

namespace CapstoneProject1.Models
{
	[DataContract]
    public class DataPoint
    {
		public DataPoint(double x, double y, double maxp, double maxn)
		{
			this.X = x;
			this.Y = y;
			this.MAXP = maxp;
			this.MAXN = maxn;
		}
		[DataMember(Name = "x")]
		public Nullable<double> X = null;

		[DataMember(Name = "y")]
		public Nullable<double> Y = null;

		[DataMember(Name = "maxp")]
		public Nullable<double> MAXP = null;

		[DataMember(Name = "maxn")]
		public Nullable<double> MAXN = null;
	}
}