using Microsoft.EntityFrameworkCore;

namespace cusho.CustomExceptions;

public class DbExceptions
{
    public static bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        // Check the inner exception message for common unique constraint error patterns
        var innerMessage = ex.InnerException?.Message ?? string.Empty;

        return innerMessage.Contains("unique", StringComparison.OrdinalIgnoreCase) ||
               innerMessage.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
               innerMessage.Contains("duplicate key", StringComparison.OrdinalIgnoreCase);
    }
}