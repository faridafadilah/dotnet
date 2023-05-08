namespace TodoApi.Base
{
  public class BasePageResponse<T>
  { 
    public int totalPage { get; set; }
    public int currentPage { get; set; }
    public long totalElement { get; set; }
    public string message { get; set; }
    public int code { get; set; }
    public T data { get; set; }
  }
}