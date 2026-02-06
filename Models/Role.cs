using System.Text.Json.Serialization;

namespace cusho.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    Admin,
    User
}