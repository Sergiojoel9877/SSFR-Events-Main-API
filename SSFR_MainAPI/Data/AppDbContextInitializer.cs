using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSFR_MainAPI.Data
{
    public class AppDbContextInitializer
    {
        public static void Initialize(AppDbContext context) {

            context.Database.EnsureCreated();

        } 
    }
}
