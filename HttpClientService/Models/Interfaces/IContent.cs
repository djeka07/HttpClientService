namespace HttpClientService.Models.Interfaces
{
    public interface IContent
    {
        object Body { get; }
        string MediaType { get; }
        string Serialize();
    }
}