using System;
using System.Collections.Generic;
using System.Text;

namespace Ultimate.SharedCommon.Messages
{
    public class CourseNameChangeEvent
    {
        public string UserId { get; set; }
        public string CourseId{ get; set; }
        public string UpdatedName { get; set; }
    }
}
