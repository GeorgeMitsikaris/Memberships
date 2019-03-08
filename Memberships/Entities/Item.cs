﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Memberships.Entities
{
    [Table("Item")]
    public class Item
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        [MaxLength(1024)]
        public string Url { get; set; }

        [MaxLength(1024)]
        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        [AllowHtml]
        public string HTML { get; set; }

        [DefaultValue(0)]
        [Display(Name = "Wait Days")]
        public int WaitDays { get; set; }

        public string HTMLShort
        {
            get
            {
                return HTML == null || HTML.Length < 50 ? HTML : HTML.Substring(0, 50);
            }
        }

        public int ProductId { get; set; }

        public int ItemTypeId { get; set; }

        public int SectionId { get; set; }

        public int PartId { get; set; }

        public bool IsFree { get; set; }

        [DisplayName("Item Types")]
        public ICollection<ItemType> ItemTypes { get; set; }

        public List<Section> Sections { get; set; }

        public List<Part> Parts { get; set; }
    }
}