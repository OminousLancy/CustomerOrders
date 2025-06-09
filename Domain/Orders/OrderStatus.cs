using System.ComponentModel;

namespace Domain.Orders;

public enum OrderStatus {
    [Description("Created")]
    Created,
    [Description("InProgress")]
    InProgress,
    [Description("Completed")]
    Completed,
    [Description("Cancelled")]
    Cancelled
}