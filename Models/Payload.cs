namespace ZeissTest.Models;

public class Payload
{
    public DateTime timestamp { get; set; }
    public string? status { get; set; }
    public string? machine_id { get; set; }
    public string? id { get; set; }
}