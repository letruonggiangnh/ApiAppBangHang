namespace ApiAppBangHang.Models.Response
{
    public class ResponseResult
    {
        public bool success { get; set; } = false;
        public string error { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        public int total { get; set; }
    }
}
