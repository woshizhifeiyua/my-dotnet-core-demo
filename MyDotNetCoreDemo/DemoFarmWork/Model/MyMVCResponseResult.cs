using System;
using System.Collections.Generic;
using System.Text;

namespace DemoFarmWork.Model
{
    public class MyMVCResponseResult
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorInfo { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public Object Result { get; set; }

        /// <summary>
        /// 状态 FAIL
        /// </summary>
        public string ActionStatus { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrorCode { get; set; }
    }
}
