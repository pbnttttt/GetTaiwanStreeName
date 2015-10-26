using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTaiwanStreetName
{
    public class CityModel
    {
        public string CityName { get; set; }
        public List<CityAreaModel> CityArea { get; set; }

        public CityModel()
        {
            CityArea = new List<CityAreaModel>();
        }
    }

    public class CityAreaModel
    {
        public string CityAreaName { get; set; }
        public List<StreetModel> Street { get; set; }

        public CityAreaModel()
        {
            Street = new List<StreetModel>();
        }
    }

    public class StreetModel
    {
        public string StreetName { get; set; }
    }
}
