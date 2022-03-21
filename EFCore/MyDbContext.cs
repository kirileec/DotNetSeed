using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore
{
    public class MyDbContext:DbContext
    {
        public static readonly Microsoft.Extensions.Logging.LoggerFactory _myLoggerFactory =
        new LoggerFactory(new[] {
            new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
        });
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
    }
}
