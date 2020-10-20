using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsService
{
    public class CustomConfiguration:ICustomConfiguration
    {
        public CustomConfiguration()
        {

        }
        public string DefaultConnectionString { get; set; }
        public string TokenAuthority { get; set; }
        public string ClientId { get; set; }
    }

    public interface ICustomConfiguration
    {
    }
}
