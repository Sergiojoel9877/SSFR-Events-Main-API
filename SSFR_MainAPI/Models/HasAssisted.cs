﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SSFR_MainAPI.Models
{
    public class HasAssisted
    {
        [Key]
        public int Id { get; set; }

        public int EventId { get; set; }

        public bool Assisted { get; set; }
    }
}
