using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Repositories.Settings
{
    public class MongoSettings : IMongoSettings
    {
        public string? Server { get; set; }
        public string? Database { get; set; }
        public string? Collection { get; set; }
    }
}