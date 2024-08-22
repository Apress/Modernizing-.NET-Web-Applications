namespace ModernizationDemo.BackendApi.Model;

public class PagedResponse<T>
{
    public List<T> Results { get; set; }

    public int TotalRecordCount { get; set; }
}