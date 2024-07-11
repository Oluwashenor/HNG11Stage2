namespace HNG11Stage2.Models
{
    public class ResponseModel<T>
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<dynamic>? Errors { get; set; } = null;

        public static ResponseModel<T> Success(T data, string message = null, int statusCode = 200)
        {
            return new ResponseModel<T> { Data = data, Message = message ?? "Successful Operation", Status = "success", StatusCode = statusCode };
        }

        public static ResponseModel<T> Error(string message = null,int statusCode = 400)
        {
            return new ResponseModel<T> { Message = message ?? "Something went wrong", Status = "Bad request", StatusCode = statusCode };
        }

        public static ResponseModel<T> MultiError(Dictionary<string, string> errors = null, string message = "Something went wrong", string status = "failed" ,int statusCode = 400)
        {
            var modifiedErrors = new List<dynamic>();   
            foreach (var item in errors)
            {
                modifiedErrors.Add(new
                {
                    field = item.Key,
                    message = item.Value,
                });
            }
            return new ResponseModel<T> { Errors = modifiedErrors, Status = status, StatusCode = statusCode, Message = message };
        }
    }


}
