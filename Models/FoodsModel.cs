﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FoodsReview.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 店家名稱
        /// </summary>
        [DisplayName("店家名稱")]
        public string Convenient { get; set; }
        /// <summary>
        /// 餐點名稱
        /// </summary>
        [DisplayName("餐點名稱")]
        public string FoodName { get; set; }
        /// <summary>
        /// 紀錄者
        /// </summary>
        [DisplayName("紀錄者")]
        public string Recorder { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [DisplayName("備註")]
        public string Memo { get; set; }
        /// <summary>
        /// 訂餐時間
        /// </summary>
        [DisplayName("訂餐時間")]
        public DateTime? RecordTime { get; set; }
    }
}
