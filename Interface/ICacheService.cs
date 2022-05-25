namespace ZeissTest.Interface;

public interface ICacheService
{
    Task<object> GetAsync(string key);
    void SetAsync(string key, object value);
}