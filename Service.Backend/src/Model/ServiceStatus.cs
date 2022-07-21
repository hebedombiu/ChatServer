using System.Text.Json.Serialization;

namespace Service.Backend.Model;

public class ServiceStatus {
    [JsonPropertyName("status")] public string Status { get; }

    [JsonConstructor]
    private ServiceStatus(string status) {
        Status = status;
    }

    public static ServiceStatus CreateOk() {
        return new ServiceStatus("ok");
    }
}