namespace TodoApi.Base
{
  public class ResponApi<T>
  {
    public string message { get; set; }
    public int code { get; set; }
    public T data { get; set; }
  }
}