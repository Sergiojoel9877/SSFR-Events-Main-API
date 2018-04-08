﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSFR_MainAPI.Models
{
    public class Guest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [StringLength(30)]
        public string Telephone { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Email{ get; set; }

        [Required]
        [StringLength(1)]
        public string Gender { get; set; }

        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Events Event { get; set; }

    }
}
