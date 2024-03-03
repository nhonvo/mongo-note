namespace api;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
}
public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
    public string DatabaseName { get; set; }
    public string BooksCollectionName { get; set; }
}
