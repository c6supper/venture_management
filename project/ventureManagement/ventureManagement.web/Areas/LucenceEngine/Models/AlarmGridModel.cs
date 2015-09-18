using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VentureManagement.Models;

namespace VentureManagement.Web.Areas.LucenceEngine.Models
{
    public class AlarmGridModel
    {
        public IEnumerable<Alarm> Alarms { get; set; }

        /// <summary>
        /// 数据库列的名称，通过它对数据进行排序
        /// </summary>
        public string SortBy { get; set; }
        /// <summary>
        /// 一个布尔值，指示是否升序排序数据
        /// </summary>
        public bool SortAscending { get; set; }

        /// <summary>
        /// 一个只读属性，返回一个排序的值的字符串SortBy SortAscending属性
        /// </summary>
        public string SortExpression
        {
            get
            {
                return this.SortAscending ? this.SortBy + " desc" : this.SortBy + " asc";
            }
        }
        
        public AlarmGridModel()
        {
            //默认值 每页显示5条记录 从第1页开始
            this.PageSize = 5;
            this.CurrentPageIndex = 1;
        }

        /// <summary>
        /// 当前页的索引
        /// </summary>
        public int CurrentPageIndex { get; set; }
        /// <summary>
        /// 每页显示的记录条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录条数
        /// </summary>
        public int TotalRecordCount { get; set; }

        /// <summary>
        /// 分页总数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (TotalRecordCount % PageSize == 0)
                {
                    return TotalRecordCount / PageSize;
                }
                else
                {
                    return TotalRecordCount / PageSize + 1;
                }
            }
        }

    }
}