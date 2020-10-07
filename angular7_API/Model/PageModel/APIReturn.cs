using System;
using System.Collections.Generic;

namespace angular_API.Model.PageModel
{
    public class APIReturn
    {
        public APIReturnCode Code { get; set; }
        public string Message { get; set; }
        public List<string> MessageList { get; set; }
        public string StackTrace { get; set; }
        public object Description { get; set; }

        public APIReturn()
        {
            this.Code = APIReturnCode.Fail;
            this.Message = String.Empty;
            this.StackTrace = String.Empty;
            this.Description = String.Empty;
        }

        public APIReturn(APIReturnCode APIReturnCode, string Message)
        {
            this.Code = APIReturnCode;
            this.Message = Message;
            this.StackTrace = String.Empty;
            this.Description = String.Empty;
        }

        public APIReturn(APIReturnCode APIReturnCode, List<string> MessageList)
        {
            this.Code = APIReturnCode;
            this.MessageList = MessageList;
            this.StackTrace = String.Empty;
            this.Description = String.Empty;
        }

        public APIReturn(APIReturnCode APIReturnCode, Exception ex)
        {
            this.Code = APIReturnCode;
            this.Message = ex.Message;
            this.StackTrace = ex.StackTrace;
            this.Description = String.Empty;
        }
    }

    public enum APIReturnCode
    {
        Exception = -1,
        Success = 0,
        Fail = 1,
    }
}
